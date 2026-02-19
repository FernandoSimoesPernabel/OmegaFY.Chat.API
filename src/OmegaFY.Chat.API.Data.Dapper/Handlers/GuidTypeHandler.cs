using Dapper;
using System.Data;

namespace OmegaFY.Chat.API.Data.Dapper.Handlers;

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