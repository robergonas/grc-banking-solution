using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class MeetingFrequencyConfiguration : IEntityTypeConfiguration<MeetingFrequency>
{
    public void Configure(EntityTypeBuilder<MeetingFrequency> builder)
    {
        builder.ToTable("MeetingFrequencies", "governance");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedNever();
        builder.Property(f => f.Name).IsRequired().HasMaxLength(100);

        builder.HasData(
            MeetingFrequency.Weekly,
            MeetingFrequency.Biweekly,
            MeetingFrequency.Monthly,
            MeetingFrequency.Quarterly,
            MeetingFrequency.Semiannually,
            MeetingFrequency.Annually,
            MeetingFrequency.AsNeeded
        );
    }
}