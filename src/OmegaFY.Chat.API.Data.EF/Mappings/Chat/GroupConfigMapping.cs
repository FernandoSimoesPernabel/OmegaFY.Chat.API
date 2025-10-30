using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmegaFY.Chat.API.Domain.Constants;
using OmegaFY.Chat.API.Domain.Entities.Chat;

namespace OmegaFY.Chat.API.Data.EF.Mappings.Chat;

internal sealed class GroupConfigMapping : IEntityTypeConfiguration<GroupConfig>
{
    public void Configure(EntityTypeBuilder<GroupConfig> builder)
    {
        builder.HasKey(group => group.Id);

        builder.Property(group => group.ConversationId).IsRequired();

        builder.Property(group => group.CreatedByUserId).IsRequired();
        
        builder.Property(group => group.GroupName).HasMaxLength(ChatConstants.MESSAGE_BODY_MAX_LENGTH).IsUnicode().IsRequired();

        builder.Property(group => group.MaxNumberOfMembers).IsRequired();

        builder.ToTable("GroupConfigs", "chat");
    }
}