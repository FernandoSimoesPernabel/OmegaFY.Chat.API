namespace OmegaFY.Chat.API.Common.Models;

public readonly record struct CursorPaginationResultInfo<T> where T : struct
{
    public T? NextCursor { get; }

    public long TotalOfItemsRemaining { get; }

    public bool HasMore => TotalOfItemsRemaining > 0;

    public CursorPaginationResultInfo(T? nextCursor, long totalOfItemsRemaining)
    {
        NextCursor = nextCursor;
        TotalOfItemsRemaining = totalOfItemsRemaining;
    }
}