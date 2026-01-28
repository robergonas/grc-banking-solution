using GRC.Identity.Domain.Aggregates.RoleAggregate;
using GRC.Identity.Domain.Aggregates.UserAggregate;
using GRC.Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Infrastructure.Contexts;

public class IdentityContextSeed
{
    public static async Task SeedAsync(
        IdentityContext context,
        IPasswordHasher passwordHasher,
        ILogger logger)
    {
        try
        {
            // Seed Roles FIRST
            if (!await context.Roles.AnyAsync())
            {
                logger.LogInformation("Seeding roles...");

                var adminRole = Role.Create("Administrator", "Administrador del sistema con acceso completo", true);
                var userRole = Role.Create("User", "Usuario estándar del sistema", true);
                var auditorRole = Role.Create("Auditor", "Auditor con permisos de solo lectura", true);

                await context.Roles.AddRangeAsync(adminRole, userRole, auditorRole);
                await context.SaveChangesAsync();

                logger.LogInformation("Roles seeded successfully");

                // Seed Permissions
                logger.LogInformation("Seeding permissions...");

                var adminPermissions = new List<Permission>
                {
                    // Users permissions
                    Permission.Create("Users", "Create"),
                    Permission.Create("Users", "Read"),
                    Permission.Create("Users", "Update"),
                    Permission.Create("Users", "Delete"),

                    // Roles permissions
                    Permission.Create("Roles", "Create"),
                    Permission.Create("Roles", "Read"),
                    Permission.Create("Roles", "Update"),
                    Permission.Create("Roles", "Delete"),

                    // Permissions permissions
                    Permission.Create("Permissions", "Create"),
                    Permission.Create("Permissions", "Read"),
                    Permission.Create("Permissions", "Update"),
                    Permission.Create("Permissions", "Delete"),

                    // Policies permissions
                    Permission.Create("Policies", "Create"),
                    Permission.Create("Policies", "Read"),
                    Permission.Create("Policies", "Update"),
                    Permission.Create("Policies", "Delete"),
                    Permission.Create("Policies", "Approve"),
                    Permission.Create("Policies", "Publish"),

                    // Committees permissions
                    Permission.Create("Committees", "Create"),
                    Permission.Create("Committees", "Read"),
                    Permission.Create("Committees", "Update"),
                    Permission.Create("Committees", "Delete"),

                    // Risks permissions
                    Permission.Create("Risks", "Create"),
                    Permission.Create("Risks", "Read"),
                    Permission.Create("Risks", "Update"),
                    Permission.Create("Risks", "Delete"),
                    Permission.Create("Risks", "Approve"),

                    // Controls permissions
                    Permission.Create("Controls", "Create"),
                    Permission.Create("Controls", "Read"),
                    Permission.Create("Controls", "Update"),
                    Permission.Create("Controls", "Delete"),
                    Permission.Create("Controls", "Execute"),

                    // Incidents permissions
                    Permission.Create("Incidents", "Create"),
                    Permission.Create("Incidents", "Read"),
                    Permission.Create("Incidents", "Update"),
                    Permission.Create("Incidents", "Delete"),

                    // Reports permissions
                    Permission.Create("Reports", "Create"),
                    Permission.Create("Reports", "Read"),
                    Permission.Create("Reports", "Execute"),
                };

                // Assign all permissions to Admin role
                foreach (var permission in adminPermissions)
                {
                    permission.AssignToRole(adminRole.Id);
                }

                await context.Permissions.AddRangeAsync(adminPermissions);

                // Create read-only permissions for Auditor role
                var auditorPermissions = new List<Permission>
                {
                    Permission.Create("Users", "Read"),
                    Permission.Create("Roles", "Read"),
                    Permission.Create("Permissions", "Read"),
                    Permission.Create("Policies", "Read"),
                    Permission.Create("Committees", "Read"),
                    Permission.Create("Risks", "Read"),
                    Permission.Create("Controls", "Read"),
                    Permission.Create("Incidents", "Read"),
                    Permission.Create("Reports", "Read"),
                };

                foreach (var permission in auditorPermissions)
                {
                    permission.AssignToRole(auditorRole.Id);
                }

                await context.Permissions.AddRangeAsync(auditorPermissions);

                // Create limited permissions for User role
                var userPermissions = new List<Permission>
                {
                    Permission.Create("Users", "Read"),
                    Permission.Create("Roles", "Read"),
                    Permission.Create("Policies", "Read"),
                    Permission.Create("Committees", "Read"),
                    Permission.Create("Risks", "Read"),
                    Permission.Create("Controls", "Read"),
                    Permission.Create("Incidents", "Create"),
                    Permission.Create("Incidents", "Read"),
                    Permission.Create("Reports", "Read"),
                };

                foreach (var permission in userPermissions)
                {
                    permission.AssignToRole(userRole.Id);
                }

                await context.Permissions.AddRangeAsync(userPermissions);
                await context.SaveChangesAsync();

                logger.LogInformation("Permissions seeded successfully");
            }

            // Seed Users - Ahora usando el constructor correcto
            if (!await context.Users.AnyAsync())
            {
                logger.LogInformation("Seeding users...");

                // Get roles
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Administrator");
                var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

                if (adminRole == null || userRole == null)
                {
                    logger.LogError("Required roles not found");
                    return;
                }

                // Create admin user - usa el constructor que acepta email, password, fullName
                var adminUser = new User("admin@grc.com", "Admin@123", "System Administrator");
                adminUser.AddRole(adminRole.Id);

                // Create regular user
                var regularUser = new User("user@grc.com", "User@123", "Regular User");
                regularUser.AddRole(userRole.Id);

                await context.Users.AddRangeAsync(adminUser, regularUser);
                await context.SaveChangesAsync();

                logger.LogInformation("Users seeded successfully");
                logger.LogInformation("Admin credentials - Email: admin@grc.com, Password: Admin@123");
                logger.LogInformation("User credentials - Email: user@grc.com, Password: User@123");
            }

            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database: {Message}", ex.Message);
            throw;
        }
    }
}