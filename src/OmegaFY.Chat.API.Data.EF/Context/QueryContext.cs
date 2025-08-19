using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OmegaFY.Chat.API.Data.EF.Context;

internal sealed class QueryContext : IdentityDbContext<IdentityUser>
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QueryContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}