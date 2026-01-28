using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;

namespace GRC.Governance.Infrastructure.EntityConfigurations;
public class CommitteeEntityTypeConfiguration : IEntityTypeConfiguration<Committee>
{
    public void Configure(EntityTypeBuilder<Committee> builder)
    {
        builder.ToTable("Committees", "governance");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion(
                v => v.Id,
                v => CommitteeType.FromValue<CommitteeType>(v))
            .HasColumnName("TypeId");

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion(
                v => v.Id,
                v => CommitteeStatus.FromValue<CommitteeStatus>(v))
            .HasColumnName("StatusId");

        builder.Property(c => c.Charter)
            .HasMaxLength(5000);

        builder.Property(c => c.ChairpersonId)
            .IsRequired();

        builder.Property(c => c.ChairpersonName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.MeetingFrequency)
            .IsRequired()
            .HasConversion(
                v => v.Id,
                v => MeetingFrequency.FromValue<MeetingFrequency>(v))
            .HasColumnName("MeetingFrequencyId");

        builder.Property(c => c.LastMeetingDate);

        builder.Property(c => c.NextMeetingDate);

        builder.Property(c => c.MinimumQuorum)
            .IsRequired()
            .HasDefaultValue(3);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        // Owned Collections - Members
        builder.OwnsMany(c => c.Members, memberBuilder =>
        {
            memberBuilder.ToTable("CommitteeMembers", "governance");

            memberBuilder.WithOwner()
                .HasForeignKey("CommitteeId");

            memberBuilder.Property<Guid>("Id");
            memberBuilder.HasKey("Id");

            memberBuilder.Property(m => m.UserId)
                .IsRequired()
                .HasColumnName("UserId");

            memberBuilder.Property(m => m.UserName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("UserName");

            memberBuilder.Property(m => m.Position)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Position");

            memberBuilder.Property(m => m.Role)
                .IsRequired()
                .HasConversion(
                    v => v.Id,
                    v => MemberRole.FromValue<MemberRole>(v))
                .HasColumnName("RoleId");

            memberBuilder.Property(m => m.JoinedDate)
                .IsRequired()
                .HasColumnName("JoinedDate");

            memberBuilder.Property(m => m.LeftDate)
                .HasColumnName("LeftDate");

            memberBuilder.Property(m => m.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("IsActive");

            memberBuilder.Property(m => m.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            memberBuilder.Property(m => m.UpdatedAt)
                .HasColumnName("UpdatedAt");

            memberBuilder.Property<Guid>("CommitteeId");
            memberBuilder.HasIndex("CommitteeId", "UserId");
        });

        // Owned Collections - Meetings
        builder.OwnsMany(c => c.Meetings, meetingBuilder =>
        {
            meetingBuilder.ToTable("CommitteeMeetings", "governance");

            meetingBuilder.WithOwner()
                .HasForeignKey("CommitteeId");

            meetingBuilder.Property<Guid>("Id");
            meetingBuilder.HasKey("Id");

            meetingBuilder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Title");

            meetingBuilder.Property(m => m.ScheduledDate)
                .IsRequired()
                .HasColumnName("ScheduledDate");

            meetingBuilder.Property(m => m.ActualStartTime)
                .HasColumnName("ActualStartTime");

            meetingBuilder.Property(m => m.ActualEndTime)
                .HasColumnName("ActualEndTime");

            meetingBuilder.Property(m => m.Agenda)
                .IsRequired()
                .HasMaxLength(5000)
                .HasColumnName("Agenda");

            meetingBuilder.Property(m => m.Location)
                .HasMaxLength(200)
                .HasColumnName("Location");

            meetingBuilder.Property(m => m.MeetingLink)
                .HasMaxLength(500)
                .HasColumnName("MeetingLink");

            meetingBuilder.Property(m => m.Status)
                .IsRequired()
                .HasConversion(
                    v => v.Id,
                    v => MeetingStatus.FromValue<MeetingStatus>(v))
                .HasColumnName("StatusId");

            meetingBuilder.Property(m => m.Minutes)
                .HasMaxLength(10000)
                .HasColumnName("Minutes");

            meetingBuilder.Property(m => m.AttendeesCount)
                .HasColumnName("AttendeesCount");

            meetingBuilder.Property(m => m.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            meetingBuilder.Property(m => m.UpdatedAt)
                .HasColumnName("UpdatedAt");

            meetingBuilder.Property<Guid>("CommitteeId");

            // Nested owned collection - Decisions
            meetingBuilder.OwnsMany(m => m.Decisions, decisionBuilder =>
            {
                decisionBuilder.ToTable("MeetingDecisions", "governance");

                decisionBuilder.WithOwner()
                    .HasForeignKey("MeetingId");

                decisionBuilder.Property<Guid>("Id");
                decisionBuilder.HasKey("Id");

                decisionBuilder.Property(d => d.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Title");

                decisionBuilder.Property(d => d.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("Description");

                decisionBuilder.Property(d => d.Type)
                    .IsRequired()
                    .HasConversion(
                        v => v.Id,
                        v => DecisionType.FromValue<DecisionType>(v))
                    .HasColumnName("TypeId");

                decisionBuilder.Property(d => d.Rationale)
                    .HasMaxLength(2000)
                    .HasColumnName("Rationale");

                decisionBuilder.Property(d => d.DecisionDate)
                    .IsRequired()
                    .HasColumnName("DecisionDate");

                decisionBuilder.Property(d => d.DecidedById)
                    .IsRequired()
                    .HasColumnName("DecidedById");

                decisionBuilder.Property(d => d.DecidedByName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DecidedByName");

                decisionBuilder.Property(d => d.CreatedAt)
                    .IsRequired()
                    .HasColumnName("CreatedAt");

                decisionBuilder.Property<Guid>("MeetingId");

                // Nested owned - VoteResult
                decisionBuilder.OwnsOne(d => d.VoteResult, voteBuilder =>
                {
                    voteBuilder.Property(v => v.VotesFor)
                        .HasColumnName("VotesFor");

                    voteBuilder.Property(v => v.VotesAgainst)
                        .HasColumnName("VotesAgainst");

                    voteBuilder.Property(v => v.Abstentions)
                        .HasColumnName("Abstentions");

                    voteBuilder.Property(v => v.Passed)
                        .HasColumnName("VotePassed");
                });
            });
        });

        // Ignore Domain Events
        builder.Ignore(c => c.DomainEvents);
    }
}