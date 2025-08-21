using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace OmegaFY.Chat.API.Data.EF.Interceptors;

internal sealed class CustomDbTransactionInterceptor : DbTransactionInterceptor
{
    public override void CreatedSavepoint(DbTransaction transaction, TransactionEventData eventData)
    {
        base.CreatedSavepoint(transaction, eventData);
    }

    public override Task CreatedSavepointAsync(DbTransaction transaction, TransactionEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.CreatedSavepointAsync(transaction, eventData, cancellationToken);
    }

    public override InterceptionResult CreatingSavepoint(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result)
    {
        return base.CreatingSavepoint(transaction, eventData, result);
    }

    public override ValueTask<InterceptionResult> CreatingSavepointAsync(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
    {
        return base.CreatingSavepointAsync(transaction, eventData, result, cancellationToken);
    }

    public override void ReleasedSavepoint(DbTransaction transaction, TransactionEventData eventData)
    {
        base.ReleasedSavepoint(transaction, eventData);
    }

    public override Task ReleasedSavepointAsync(DbTransaction transaction, TransactionEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.ReleasedSavepointAsync(transaction, eventData, cancellationToken);
    }

    public override InterceptionResult ReleasingSavepoint(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result)
    {
        return base.ReleasingSavepoint(transaction, eventData, result);
    }

    public override ValueTask<InterceptionResult> ReleasingSavepointAsync(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
    {
        return base.ReleasingSavepointAsync(transaction, eventData, result, cancellationToken);
    }

    public override void RolledBackToSavepoint(DbTransaction transaction, TransactionEventData eventData)
    {
        base.RolledBackToSavepoint(transaction, eventData);
    }

    public override Task RolledBackToSavepointAsync(DbTransaction transaction, TransactionEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.RolledBackToSavepointAsync(transaction, eventData, cancellationToken);
    }

    public override InterceptionResult RollingBackToSavepoint(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result)
    {
        return base.RollingBackToSavepoint(transaction, eventData, result);
    }

    public override ValueTask<InterceptionResult> RollingBackToSavepointAsync(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
    {
        return base.RollingBackToSavepointAsync(transaction, eventData, result, cancellationToken);
    }

    public override void TransactionCommitted(DbTransaction transaction, TransactionEndEventData eventData)
    {
        base.TransactionCommitted(transaction, eventData);
    }

    public override Task TransactionCommittedAsync(DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.TransactionCommittedAsync(transaction, eventData, cancellationToken);
    }

    public override InterceptionResult TransactionCommitting(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result)
    {
        return base.TransactionCommitting(transaction, eventData, result);
    }

    public override ValueTask<InterceptionResult> TransactionCommittingAsync(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
    {
        return base.TransactionCommittingAsync(transaction, eventData, result, cancellationToken);
    }

    public override void TransactionFailed(DbTransaction transaction, TransactionErrorEventData eventData)
    {
        base.TransactionFailed(transaction, eventData);
    }

    public override Task TransactionFailedAsync(DbTransaction transaction, TransactionErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.TransactionFailedAsync(transaction, eventData, cancellationToken);
    }

    public override void TransactionRolledBack(DbTransaction transaction, TransactionEndEventData eventData)
    {
        base.TransactionRolledBack(transaction, eventData);
    }

    public override Task TransactionRolledBackAsync(DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.TransactionRolledBackAsync(transaction, eventData, cancellationToken);
    }

    public override InterceptionResult TransactionRollingBack(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result)
    {
        return base.TransactionRollingBack(transaction, eventData, result);
    }

    public override ValueTask<InterceptionResult> TransactionRollingBackAsync(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
    {
        return base.TransactionRollingBackAsync(transaction, eventData, result, cancellationToken);
    }

    public override DbTransaction TransactionStarted(DbConnection connection, TransactionEndEventData eventData, DbTransaction result)
    {
        return base.TransactionStarted(connection, eventData, result);
    }

    public override ValueTask<DbTransaction> TransactionStartedAsync(DbConnection connection, TransactionEndEventData eventData, DbTransaction result, CancellationToken cancellationToken = default)
    {
        return base.TransactionStartedAsync(connection, eventData, result, cancellationToken);
    }

    public override InterceptionResult<DbTransaction> TransactionStarting(DbConnection connection, TransactionStartingEventData eventData, InterceptionResult<DbTransaction> result)
    {
        return base.TransactionStarting(connection, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbTransaction>> TransactionStartingAsync(DbConnection connection, TransactionStartingEventData eventData, InterceptionResult<DbTransaction> result, CancellationToken cancellationToken = default)
    {
        return base.TransactionStartingAsync(connection, eventData, result, cancellationToken);
    }

    public override DbTransaction TransactionUsed(DbConnection connection, TransactionEventData eventData, DbTransaction result)
    {
        return base.TransactionUsed(connection, eventData, result);
    }

    public override ValueTask<DbTransaction> TransactionUsedAsync(DbConnection connection, TransactionEventData eventData, DbTransaction result, CancellationToken cancellationToken = default)
    {
        return base.TransactionUsedAsync(connection, eventData, result, cancellationToken);
    }
}