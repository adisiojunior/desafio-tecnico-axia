namespace DesafioTecnicoAxia.Domain.Entidades;

public abstract class BaseEntity
{
    public Guid Id { get; internal set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; internal set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; internal set; }
    public bool IsDeleted { get; internal set; } = false;
    public DateTime? DeletedAt { get; internal set; }

    protected BaseEntity()
    {
    }

    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
    }
}

