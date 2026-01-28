using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class CommitteeTypeConfiguration : IEntityTypeConfiguration<CommitteeType>
{
    public void Configure(EntityTypeBuilder<CommitteeType> builder)
    {
        builder.ToTable("CommitteeTypes", "governance");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

        builder.HasData(
            CommitteeType.Board,
            CommitteeType.Audit,
            CommitteeType.Risk,
            CommitteeType.Compliance,
            CommitteeType.Ethics,
            CommitteeType.IT,
            CommitteeType.Investment,
            CommitteeType.Executive
        );
    }
}