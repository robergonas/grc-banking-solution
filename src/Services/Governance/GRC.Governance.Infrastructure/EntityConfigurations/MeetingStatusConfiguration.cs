using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class MeetingStatusConfiguration : IEntityTypeConfiguration<MeetingStatus>
{
    public void Configure(EntityTypeBuilder<MeetingStatus> builder)
    {
        builder.ToTable("MeetingStatuses", "governance");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);

        builder.HasData(
            MeetingStatus.Scheduled,
            MeetingStatus.InProgress,
            MeetingStatus.Completed,
            MeetingStatus.Cancelled
        );
    }
}