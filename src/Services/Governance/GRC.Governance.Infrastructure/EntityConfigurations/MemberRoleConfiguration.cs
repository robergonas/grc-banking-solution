using GRC.Governance.Domain.Aggregates.CommitteeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class MemberRoleConfiguration : IEntityTypeConfiguration<MemberRole>
{
    public void Configure(EntityTypeBuilder<MemberRole> builder)
    {
        builder.ToTable("MemberRoles", "governance");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();
        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);

        builder.HasData(
            MemberRole.Chairperson,
            MemberRole.ViceChair,
            MemberRole.Secretary,
            MemberRole.Member,
            MemberRole.Observer
        );
    }
}