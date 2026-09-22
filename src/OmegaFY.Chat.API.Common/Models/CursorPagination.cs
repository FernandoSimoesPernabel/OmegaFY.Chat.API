namespace OmegaFY.Chat.API.Common.Models;

public sealed record CursorPagination<T> where T : struct
{
    public int Take { get; init; } = 50;

    public T? Cursor { get; init; }

    public CursorPagination() { }

    public CursorPagination(int take, T? cursor)
    {
        Take = take;
        Cursor = cursor;
    }
}