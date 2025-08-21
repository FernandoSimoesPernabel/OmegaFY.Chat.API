using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OmegaFY.Chat.API.Data.EF.Context;

internal sealed class ApplicationContext : IdentityDbContext<IdentityUser>
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}