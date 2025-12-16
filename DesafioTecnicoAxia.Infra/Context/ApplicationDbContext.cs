using Microsoft.EntityFrameworkCore;
using DesafioTecnicoAxia.Domain.Entidades;

namespace DesafioTecnicoAxia.Infra.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Veiculo> Veiculos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Descricao)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(e => e.Marca)
                .IsRequired()
                .HasConversion<int>();
            
            entity.Property(e => e.Modelo)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Opcionais)
                .HasMaxLength(1000);
            
            entity.Property(e => e.Valor)
                .HasPrecision(18, 2);
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();
            
            entity.Property(e => e.UpdatedAt);
            
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);
            
            entity.Property(e => e.DeletedAt);
            
            entity.HasIndex(e => e.Marca)
                .HasDatabaseName("IX_Veiculos_Marca");
            
            entity.HasIndex(e => e.Modelo)
                .HasDatabaseName("IX_Veiculos_Modelo");
            
            entity.HasIndex(e => e.Valor)
                .HasDatabaseName("IX_Veiculos_Valor");
            
            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("IX_Veiculos_CreatedAt");
            
            entity.HasIndex(e => new { e.IsDeleted, e.Marca })
                .HasDatabaseName("IX_Veiculos_IsDeleted_Marca");
            
            entity.HasIndex(e => new { e.IsDeleted, e.Valor })
                .HasDatabaseName("IX_Veiculos_IsDeleted_Valor");
        });
    }
}

