using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmegaFY.Chat.API.Domain.Constants;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Chat;

namespace OmegaFY.Chat.API.Data.EF.Mappings.Chat;

internal sealed class MessageMapping : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(message => message.Id);

        builder.Property(message => message.ConversationId).IsRequired();

        builder.Property(message => message.SenderMemberId).IsRequired();

        builder.Property(message => message.SendDate).IsRequired();

        builder.Property(message => message.Type).HasConversion<string>().HasMaxLength(20).IsUnicode(false).IsRequired();

        builder.ComplexProperty(message => message.Body, navigationBuilder =>
        {
            navigationBuilder.Property(body => body.Content).HasColumnName(nameof(MessageBody.Content)).HasMaxLength(ChatConstants.MESSAGE_BODY_MAX_LENGTH).IsUnicode(true).IsRequired();
        });

        builder.ToTable("Messages", "chat");
    }
}