namespace OmegaFY.Chat.API.Common.Models;

public sealed record CursorPaginationResultInfo<T> : PaginationResultInfo where T : struct
{
    public T? NextCursor { get; init; }

    public CursorPaginationResultInfo(int take, T? nextCursor) : base(1, take, 0)
    {
        NextCursor = nextCursor;
    }
}
