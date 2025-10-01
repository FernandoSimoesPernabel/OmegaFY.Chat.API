using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmegaFY.Chat.API.Domain.Constants;
using OmegaFY.Chat.API.Domain.Entities.Users;

namespace OmegaFY.Chat.API.Data.EF.Mappings.Users;

internal sealed  class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.HasIndex(user => user.Email).IsUnique();

        builder.Property(user => user.Id).IsRequired().ValueGeneratedNever();

        builder.Property(user => user.Email).HasMaxLength(UserConstants.MAX_EMAIL_LENGTH).IsUnicode(false).IsRequired();

        builder.Property(user => user.DisplayName).HasMaxLength(UserConstants.MAX_DISPLAY_NAME_LENGTH).IsUnicode(false).IsUnicode(false).IsRequired();

        builder.ToTable("Users", "chat");
    }
}