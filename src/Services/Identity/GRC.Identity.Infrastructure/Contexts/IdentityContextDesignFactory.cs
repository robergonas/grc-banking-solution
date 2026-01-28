using GRC.Identity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class IdentityContextDesignFactory : IDesignTimeDbContextFactory<IdentityContext>
{
    public IdentityContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();

        optionsBuilder.UseSqlServer(
            //"Server=MSI\\DEVSQL2022;Database=GRC_Identity_DB;User Id=sa;Password=R0b3r25@;TrustServerCertificate=True;",
            "Server=MSI\\DEVSQL2022,60012;Database=GRC_Identity_dev;User Id=sa;Password=R0b3r25@;TrustServerCertificate=True;MultipleActiveResultSets=true",
            sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "identity");
            });

        return new IdentityContext(optionsBuilder.Options);
    }
}