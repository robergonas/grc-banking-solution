using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class CommitteeStatusConfiguration : IEntityTypeConfiguration<CommitteeStatus>
{
    public void Configure(EntityTypeBuilder<CommitteeStatus> builder)
    {
        builder.ToTable("CommitteeStatuses", "governance");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);

        builder.HasData(
            CommitteeStatus.Active,
            CommitteeStatus.Suspended,
            CommitteeStatus.Dissolved
        );
    }
}