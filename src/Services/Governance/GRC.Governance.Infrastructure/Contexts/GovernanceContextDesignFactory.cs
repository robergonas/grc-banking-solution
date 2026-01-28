using GRC.Governance.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class GovernanceContextDesignFactory : IDesignTimeDbContextFactory<GovernanceContext>
{
    public GovernanceContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GovernanceContext>();

        optionsBuilder.UseSqlServer(
            "Server=MSI\\DEVSQL2022,60012;Database=GRC_Governance_DB;User Id=sa;Password=R0b3r25@;TrustServerCertificate=True;",
            sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "governance");
            });

        return new GovernanceContext(optionsBuilder.Options);
    }
}