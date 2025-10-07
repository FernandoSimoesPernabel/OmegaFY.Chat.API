using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.Data.EF.ValueConverts;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Data.EF.Context;

internal sealed class ApplicationContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

        RegisterNoKeyModels(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<ReferenceId>().HaveConversion<ReferenceIdValueConverter>();
        
        base.ConfigureConventions(configurationBuilder);
    }

    private void RegisterNoKeyModels(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GetCurrentUserInfoQueryResult>().HasNoKey();

        modelBuilder.Entity<FriendshipModel>(builder =>
        {
            builder.Property(friendship => friendship.FriendshipId).IsRequired().ValueGeneratedNever();

            builder.Property(friendship => friendship.RequestingUserId).IsRequired();

            builder.Property(friendship => friendship.InvitedUserId).IsRequired();

            builder.Property(friendship => friendship.StartedDate).IsRequired();

            builder.Property(friendship => friendship.Status).HasColumnType("varchar(10)").IsRequired();

            builder.HasNoKey();
        });
    }   
}