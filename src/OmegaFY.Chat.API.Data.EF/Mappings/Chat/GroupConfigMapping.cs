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

        builder.HasIndex(group => group.ConversationId).IsUnique();

        builder.Property(group => group.Id).IsRequired().ValueGeneratedNever();

        builder.Property(group => group.ConversationId).IsRequired();

        builder.Property(group => group.CreatedByUserId).IsRequired();
        
        builder.Property(group => group.GroupName).HasMaxLength(ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH).IsUnicode().IsRequired();

        builder.Property(group => group.MaxNumberOfMembers).IsRequired();

        builder.ToTable("GroupConfigs");
    }
}