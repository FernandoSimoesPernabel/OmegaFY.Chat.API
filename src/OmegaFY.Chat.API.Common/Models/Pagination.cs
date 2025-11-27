namespace OmegaFY.Chat.API.Common.Models;

public sealed record class Pagination
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 50;

    public Pagination() { }

    public Pagination(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}