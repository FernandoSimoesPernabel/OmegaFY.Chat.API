using Microsoft.EntityFrameworkCore.Diagnostics;

namespace OmegaFY.Chat.API.Data.EF.Interceptors;

internal sealed class CustomSaveChangesInterceptor : SaveChangesInterceptor
{
    public override void SaveChangesCanceled(DbContextEventData eventData)
    {
        base.SaveChangesCanceled(eventData);
    }

    public override Task SaveChangesCanceledAsync(DbContextEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.SaveChangesCanceledAsync(eventData, cancellationToken);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        return base.SavedChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult ThrowingConcurrencyException(ConcurrencyExceptionEventData eventData, InterceptionResult result)
    {
        return base.ThrowingConcurrencyException(eventData, result);
    }

    public override ValueTask<InterceptionResult> ThrowingConcurrencyExceptionAsync(ConcurrencyExceptionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
    {
        return base.ThrowingConcurrencyExceptionAsync(eventData, result, cancellationToken);
    }
}