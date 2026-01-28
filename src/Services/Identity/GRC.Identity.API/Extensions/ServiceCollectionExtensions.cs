using FluentValidation;
using GRC.Identity.Application.Commands.ChangePassword;
using GRC.Identity.Application.Commands.LoginUser;
//using GRC.Identity.Domain.Services;
using GRC.Identity.Infrastructure.Extensions;
using GRC.Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace GRC.Identity.API.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Controllers
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        // Swagger/OpenAPI
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "GRC Identity API",
                Version = "v1",
                Description = "API de Gestión de Identidad y Acceso",
                Contact = new OpenApiContact
                {
                    Name = "GRC Team",
                    Email = "support@grc.com"
                }
            });

            // Configuración JWT en Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando el esquema Bearer. \r\n\r\n" +
                              "Ingrese SOLO el token (sin la palabra 'Bearer').\r\n\r\n" +
                              "Ejemplo: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Incluir comentarios XML si existen
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", builder =>
            {
                builder.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:4200",
                    "https://localhost:7002"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });

        // Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<Infrastructure.Contexts.IdentityContext>();

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly);
        });

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(LoginUserCommandValidator).Assembly);

        // AutoMapper
        services.AddAutoMapper(typeof(Application.Mappings.IdentityMappingProfile).Assembly);

        // Infrastructure Services (esto incluye DbContext, Repositories Y Authentication)
        services.AddInfrastructureServices(configuration);

        // Domain Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

        return services;
    }
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<Middleware.ExceptionHandlingMiddleware>();
    }
}