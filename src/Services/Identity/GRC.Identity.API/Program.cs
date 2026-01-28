using GRC.Identity.API.Extensions;
using GRC.Identity.API.Middleware;
using GRC.Identity.Infrastructure.Contexts;
using GRC.Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/identity-api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting GRC Identity API");

    // Agregar TODOS los servicios a través del método de extensión
    // Esto incluye: Controllers, Swagger, CORS, HealthChecks, MediatR, 
    // FluentValidation, AutoMapper, Infrastructure y Authentication
    builder.Services.AddApiServices(builder.Configuration);

    var app = builder.Build();

    // Configurar el pipeline HTTP
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "GRC Identity API v1");
            c.RoutePrefix = string.Empty;
        });
    }

    // ORDEN IMPORTANTE del middleware
    app.UseExceptionHandlingMiddleware();
    app.UseHttpsRedirection();
    app.UseCors("AllowSpecificOrigins");

    // Authentication DEBE ir antes de Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Endpoints
    app.MapControllers();
    app.MapHealthChecks("/health");

    // Inicializar base de datos y seed
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<IdentityContext>();
            var passwordHasher = services.GetRequiredService<IPasswordHasher>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Checking database connection...");

            var canConnect = await context.Database.CanConnectAsync();
            if (!canConnect)
            {
                logger.LogWarning("Cannot connect to database. Please verify SQL Server is running.");
            }
            else
            {
                logger.LogInformation("Database connection successful");
                logger.LogInformation("Applying database migrations...");

                await context.Database.MigrateAsync();
                logger.LogInformation("Database migrations applied successfully");

                logger.LogInformation("Seeding database...");
                await IdentityContextSeed.SeedAsync(context, passwordHasher, logger);
                logger.LogInformation("Database seeded successfully");
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database");
            logger.LogWarning("Application will start but database may not be properly initialized");
        }
    }

    Log.Information("=================================================");
    Log.Information("GRC Identity API started successfully");
    Log.Information("Swagger UI: https://localhost:7002/");
    Log.Information("Health Check: https://localhost:7002/health");
    Log.Information("=================================================");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}