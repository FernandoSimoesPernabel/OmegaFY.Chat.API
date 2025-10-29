using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmegaFY.Chat.API.Domain.Entities.Chat;

namespace OmegaFY.Chat.API.Data.EF.Mappings.Chat;

internal sealed class ConversationMapping : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(conversation => conversation.Id);

        builder.Property(conversation => conversation.Type).HasMaxLength(15).IsUnicode(false).IsRequired();

        builder.Property(conversation => conversation.Status).HasMaxLength(10).IsUnicode(false).IsRequired();

        builder.Property(conversation => conversation.CreatedDate).IsRequired();

        builder.HasOne(conversation => conversation.GroupConfig).WithOne().HasForeignKey<GroupConfig>(group => group.ConversationId);

        builder.HasMany(conversation => conversation.Members).WithOne().HasForeignKey(member => member.ConversationId);

        builder.ToTable("Conversations", "chat");
    }
}