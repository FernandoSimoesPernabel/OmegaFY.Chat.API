using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmegaFY.Chat.API.Domain.Entities.Users;

namespace OmegaFY.Chat.API.Data.EF.Mappings.Users;

internal sealed class FriendshipMapping : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.HasKey(friendship => friendship.Id);

        builder.Property(friendship => friendship.Id).IsRequired().ValueGeneratedNever();

        builder.Property(friendship => friendship.RequestingUserId).IsRequired();

        builder.Property(friendship => friendship.InvitedUserId).IsRequired();

        builder.Property(friendship => friendship.StartedDate).IsRequired();

        builder.Property(friendship => friendship.Status).HasColumnType("varchar(10)").IsRequired();

        builder.ToTable("Friendships", "chat");
    }
}