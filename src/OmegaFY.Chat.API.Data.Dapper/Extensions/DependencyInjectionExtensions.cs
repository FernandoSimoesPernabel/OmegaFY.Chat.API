using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Data.Dapper.QueryProviders.Chat;
using OmegaFY.Chat.API.Data.Dapper.QueryProviders.Users;
using System.Data;

namespace OmegaFY.Chat.API.Data.Dapper.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDapperQueryProviders(this IServiceCollection services, IConfigurationRoot configuration)
    {
        SqlMapper.AddTypeHandler(new GuidTypeHandler());

        services.AddTransient<IDbConnection>(_ => new SqliteConnection(configuration.GetConnectionString("Sqlite")));

        services.AddScoped<IChatQueryProvider, ChatQueryProvider>();

        services.AddScoped<IUserQueryProvider, UserQueryProvider>();

        return services;
    }
}

internal sealed class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value)
    {
        return value switch
        {
            string stringValue => Guid.Parse(stringValue),
            Guid guidValue => guidValue,
            _ => Guid.Empty
        };
    }

    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
        parameter.DbType = DbType.String;
    }
}