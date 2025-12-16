using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Linq;
using DesafioTecnicoAxia.Infra.Context;
using DesafioTecnicoAxia.Infra.Repository;
using DesafioTecnicoAxia.Infra.UnitOfWork;
using DesafioTecnicoAxia.Application.VeiculoService;
using DesafioTecnicoAxia.Application.Validators;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Application.Mappings;
using DesafioTecnicoAxia.WebApi.Middleware;

namespace DesafioTecnicoAxia.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Desafio Técnico Axia - API de Veículos",
        Version = "v1",
        Description = "API REST para cadastro e consulta de veículos"
    });
    
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly("DesafioTecnicoAxia.Infra");
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
    }));

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, tags: new[] { "ready", "db", "postgresql" })
    .AddDbContextCheck<ApplicationDbContext>(tags: new[] { "ready", "db" });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVeiculoRepository, VeiculoRepository>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AdicionarVeiculoCommand).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<AdicionarVeiculoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = System.Threading.RateLimiting.PartitionedRateLimiter.Create<HttpContext, string>(context =>
        System.Threading.RateLimiting.RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new System.Threading.RateLimiting.FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
    
    options.AddPolicy("api", context =>
        System.Threading.RateLimiting.RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new System.Threading.RateLimiting.FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(1)
            }));
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseResponseCaching();
app.UseRateLimiter();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers().RequireRateLimiting("api");
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
});

try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    var canConnect = await dbContext.Database.CanConnectAsync();
    logger.LogInformation("Pode conectar ao banco: {CanConnect}", canConnect);
    
    var databaseCreated = await dbContext.Database.EnsureCreatedAsync();
    if (databaseCreated)
    {
        logger.LogInformation("Banco de dados criado.");
    }
    
    if (dbContext.Database.IsRelational())
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        var pendingMigrationsList = pendingMigrations.ToList();
        
        if (pendingMigrationsList.Any())
        {
            logger.LogInformation("Aplicando {Count} migrations pendentes...", pendingMigrationsList.Count);
            foreach (var migration in pendingMigrationsList)
            {
                logger.LogInformation("  - {Migration}", migration);
            }
            
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Migrations aplicadas com sucesso.");
        }
        else
        {
            logger.LogInformation("Nenhuma migration pendente.");
            var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
            var appliedMigrationsList = appliedMigrations.ToList();
            logger.LogInformation("Migrations aplicadas: {Count}", appliedMigrationsList.Count);
            
            if (!appliedMigrationsList.Any())
            {
                logger.LogWarning("Nenhuma migration foi aplicada ainda. Aplicando todas as migrations...");
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Migrations aplicadas.");
            }
        }
    }
    else
    {
        logger.LogInformation("Provider nao relacional detectado (ex: InMemory). Pulando migrations.");
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Erro ao aplicar migrations: {Message}. InnerException: {InnerException}", 
        ex.Message, ex.InnerException?.Message);
    
    if (app.Environment.IsDevelopment())
    {
        throw;
    }
}

        await app.RunAsync();
    }
}
