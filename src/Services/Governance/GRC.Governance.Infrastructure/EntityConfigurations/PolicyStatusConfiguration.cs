using GRC.Governance.Domain.Aggregates.PolicyAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PolicyStatusConfiguration : IEntityTypeConfiguration<PolicyStatus>
{
    public void Configure(EntityTypeBuilder<PolicyStatus> builder)
    {
        builder.ToTable("PolicyStatuses", "governance");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Seed data
        builder.HasData(
            PolicyStatus.Draft,
            PolicyStatus.PendingApproval,
            PolicyStatus.Approved,
            PolicyStatus.Active,
            PolicyStatus.UnderReview,
            PolicyStatus.Expired,
            PolicyStatus.Archived
        );
    }
}