using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Identity.Domain.Aggregates.RoleAggregate;
using GRC.Identity.Domain.Aggregates.UserAggregate;
using GRC.Identity.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace GRC.Identity.Infrastructure.Contexts;

public class IdentityContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());

        // Asegurar que Permission NO es owned type
        modelBuilder.Entity<Permission>().HasKey(p => p.Id);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}