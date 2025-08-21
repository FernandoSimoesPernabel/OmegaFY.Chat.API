using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace OmegaFY.Chat.API.Data.EF.Interceptors;

internal sealed class CustomDbConnectionInterceptor : DbConnectionInterceptor
{
    public override void ConnectionClosed(DbConnection connection, ConnectionEndEventData eventData)
    {
        base.ConnectionClosed(connection, eventData);
    }

    public override Task ConnectionClosedAsync(DbConnection connection, ConnectionEndEventData eventData)
    {
        return base.ConnectionClosedAsync(connection, eventData);
    }

    public override InterceptionResult ConnectionClosing(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        return base.ConnectionClosing(connection, eventData, result);
    }

    public override ValueTask<InterceptionResult> ConnectionClosingAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        return base.ConnectionClosingAsync(connection, eventData, result);
    }

    public override DbConnection ConnectionCreated(ConnectionCreatedEventData eventData, DbConnection result)
    {
        return base.ConnectionCreated(eventData, result);
    }

    public override InterceptionResult<DbConnection> ConnectionCreating(ConnectionCreatingEventData eventData, InterceptionResult<DbConnection> result)
    {
        return base.ConnectionCreating(eventData, result);
    }

    public override void ConnectionDisposed(DbConnection connection, ConnectionEndEventData eventData)
    {
        base.ConnectionDisposed(connection, eventData);
    }

    public override Task ConnectionDisposedAsync(DbConnection connection, ConnectionEndEventData eventData)
    {
        return base.ConnectionDisposedAsync(connection, eventData);
    }

    public override InterceptionResult ConnectionDisposing(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        return base.ConnectionDisposing(connection, eventData, result);
    }

    public override ValueTask<InterceptionResult> ConnectionDisposingAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        return base.ConnectionDisposingAsync(connection, eventData, result);
    }

    public override void ConnectionFailed(DbConnection connection, ConnectionErrorEventData eventData)
    {
        base.ConnectionFailed(connection, eventData);
    }

    public override Task ConnectionFailedAsync(DbConnection connection, ConnectionErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.ConnectionFailedAsync(connection, eventData, cancellationToken);
    }

    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        base.ConnectionOpened(connection, eventData);
    }

    public override Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        return base.ConnectionOpening(connection, eventData, result);
    }

    public override ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
    {
        return base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
    }
}