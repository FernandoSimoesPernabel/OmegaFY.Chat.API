using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OmegaFY.Chat.API.Data.EF.ValueConverts;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Data.EF.Context;

internal sealed class ApplicationContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<ReferenceId>().HaveConversion<ReferenceIdValueConverter>();
        
        base.ConfigureConventions(configurationBuilder);
    }
}