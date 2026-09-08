using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmegaFY.Chat.API.Domain.Entities.Chat;

namespace OmegaFY.Chat.API.Data.EF.Mappings.Chat;

internal sealed class MemberMapping : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(member => member.Id);

        builder.HasIndex(member => new { member.ConversationId, member.UserId }).IsUnique();

        builder.Property(member => member.Id).IsRequired().ValueGeneratedNever();

        builder.Property(member => member.ConversationId).IsRequired();

        builder.Property(member => member.UserId).IsRequired();

        builder.Property(member => member.JoinedDate).IsRequired();

        builder.ToTable("Members");
    }
}