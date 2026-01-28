using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class DecisionTypeConfiguration : IEntityTypeConfiguration<DecisionType>
{
    public void Configure(EntityTypeBuilder<DecisionType> builder)
    {
        builder.ToTable("DecisionTypes", "governance");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

        builder.HasData(
            DecisionType.PolicyApproval,
            DecisionType.RiskAcceptance,
            DecisionType.BudgetApproval,
            DecisionType.Strategic,
            DecisionType.Operational,
            DecisionType.Compliance
        );
    }
}