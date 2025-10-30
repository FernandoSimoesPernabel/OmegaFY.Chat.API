using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmegaFY.Chat.API.Domain.Entities.Chat;

namespace OmegaFY.Chat.API.Data.EF.Mappings.Chat;

internal sealed class MemberMessageMapping : IEntityTypeConfiguration<MemberMessage>
{
    public void Configure(EntityTypeBuilder<MemberMessage> builder)
    {
        builder.HasKey(message => message.Id);

        builder.Property(message => message.MessageId).IsRequired();

        builder.Property(message => message.SenderMemberId).IsRequired();

        builder.Property(message => message.DestinationMemberId).IsRequired();

        builder.Property(message => message.DeliveryDate).IsRequired();

        builder.Property(message => message.Status).HasConversion<string>().HasMaxLength(20).IsUnicode(false).IsRequired();

        builder.ToTable("MemberMessages", "chat");
    }
}