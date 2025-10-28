using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmegaFY.Chat.API.Domain.Entities.Chat;

namespace OmegaFY.Chat.API.Data.EF.Mappings.Chat;

internal sealed class MemberMapping : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(member => member.Id);

        builder.Property(member => member.ConversationId).IsRequired();

        builder.Property(member => member.UserId).IsRequired();

        builder.Property(member => member.JoinedDate).IsRequired();

        builder.ToTable("Members", "chat");
    }
}