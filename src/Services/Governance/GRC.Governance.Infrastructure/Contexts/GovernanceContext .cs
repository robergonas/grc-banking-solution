using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.PolicyAggregate;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;
using GRC.Governance.Infrastructure.EntityConfigurations;

namespace GRC.Governance.Infrastructure.Contexts;

public class GovernanceContext : DbContext, IUnitOfWork
{
    public DbSet<Policy> Policies { get; set; }
    public DbSet<Committee> Committees { get; set; }
    public DbSet<PolicyType> PolicyTypes { get; set; }
    public DbSet<PolicyStatus> PolicyStatuses { get; set; }
    public DbSet<CommitteeType> CommitteeTypes { get; set; }
    public DbSet<CommitteeStatus> CommitteeStatuses { get; set; }
    public DbSet<MeetingFrequency> MeetingFrequencies { get; set; }
    public DbSet<MemberRole> MemberRoles { get; set; }
    public DbSet<MeetingStatus> MeetingStatuses { get; set; }
    public DbSet<DecisionType> DecisionTypes { get; set; }
    public GovernanceContext(DbContextOptions<GovernanceContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PolicyEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PolicyTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PolicyStatusConfiguration());
        modelBuilder.ApplyConfiguration(new CommitteeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CommitteeTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CommitteeStatusConfiguration());
        modelBuilder.ApplyConfiguration(new MeetingFrequencyConfiguration());
        modelBuilder.ApplyConfiguration(new MemberRoleConfiguration());
        modelBuilder.ApplyConfiguration(new MeetingStatusConfiguration());
        modelBuilder.ApplyConfiguration(new DecisionTypeConfiguration());
    }
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result > 0;
    }
}