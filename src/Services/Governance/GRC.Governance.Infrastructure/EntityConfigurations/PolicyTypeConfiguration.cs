using GRC.Governance.Domain.Aggregates.PolicyAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PolicyTypeConfiguration : IEntityTypeConfiguration<PolicyType>
{
    public void Configure(EntityTypeBuilder<PolicyType> builder)
    {
        builder.ToTable("PolicyTypes", "governance");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Seed data
        builder.HasData(
            PolicyType.Corporate,
            PolicyType.Operational,
            PolicyType.Security,
            PolicyType.Compliance,
            PolicyType.Risk,
            PolicyType.IT,
            PolicyType.HR,
            PolicyType.Financial
        );
    }
}