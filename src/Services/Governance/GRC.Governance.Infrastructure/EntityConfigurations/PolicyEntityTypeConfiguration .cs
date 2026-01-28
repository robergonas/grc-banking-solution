using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GRC.Governance.Domain.Aggregates.PolicyAggregate;

namespace GRC.Governance.Infrastructure.EntityConfigurations;
public class PolicyEntityTypeConfiguration : IEntityTypeConfiguration<Policy>
{
    public void Configure(EntityTypeBuilder<Policy> builder)
    {
        builder.ToTable("Policies", "governance");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.Type)
            .IsRequired()
            .HasConversion(
                v => v.Id,
                v => PolicyType.FromValue<PolicyType>(v))
            .HasColumnName("TypeId");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion(
                v => v.Id,
                v => PolicyStatus.FromValue<PolicyStatus>(v))
            .HasColumnName("StatusId");

        builder.Property(p => p.EffectiveDate)
            .IsRequired();

        builder.Property(p => p.ExpirationDate);

        builder.Property(p => p.Version)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(p => p.ParentPolicyId);

        builder.Property(p => p.OwnerId)
            .IsRequired();

        builder.Property(p => p.OwnerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.ApprovedById);

        builder.Property(p => p.ApprovedDate);

        builder.Property(p => p.ApprovalComments)
            .HasMaxLength(1000);

        builder.Property(p => p.ReviewCycleMonths)
            .IsRequired();

        builder.Property(p => p.LastReviewDate);

        builder.Property(p => p.NextReviewDate);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);

        // Owned Collections - Documents
        builder.OwnsMany(p => p.Documents, docBuilder =>
        {
            docBuilder.ToTable("PolicyDocuments", "governance");

            docBuilder.WithOwner()
                .HasForeignKey("PolicyId");

            docBuilder.Property<Guid>("Id");
            docBuilder.HasKey("Id");

            docBuilder.Property(d => d.FileName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("FileName");

            docBuilder.Property(d => d.FileUrl)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("FileUrl");

            docBuilder.Property(d => d.DocumentType)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("DocumentType");

            docBuilder.Property(d => d.FileSize)
                .HasColumnName("FileSize");

            docBuilder.Property(d => d.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            docBuilder.Property<Guid>("PolicyId");
        });

        // Owned Collections - Tags
        builder.OwnsMany(p => p.Tags, tagBuilder =>
        {
            tagBuilder.ToTable("PolicyTags", "governance");

            tagBuilder.WithOwner()
                .HasForeignKey("PolicyId");

            tagBuilder.Property<Guid>("Id")
                .ValueGeneratedOnAdd();

            tagBuilder.HasKey("Id");

            tagBuilder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("TagName");

            tagBuilder.HasIndex("TagName");

            tagBuilder.Property<Guid>("PolicyId");
        });

        // Ignore Domain Events
        builder.Ignore(p => p.DomainEvents);
    }
}