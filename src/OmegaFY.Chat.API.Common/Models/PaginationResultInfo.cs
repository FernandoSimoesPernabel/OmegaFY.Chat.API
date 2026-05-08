namespace OmegaFY.Chat.API.Common.Models;

public readonly record struct PaginationResultInfo
{
    public int CurrentPage { get; }

    public int PageSize { get; }

    public int TotalPages { get; }

    public long TotalOfItems { get; }

    public bool HasPrevious => TotalPages > 0 && CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;

    public PaginationResultInfo(Pagination pagination, long totalOfItems) : this(pagination.PageNumber, pagination.PageSize, totalOfItems) { }

    public PaginationResultInfo(int currentPage, int pageSize, long totalOfItems)
    {
        CurrentPage = Math.Max(currentPage, 1);
        PageSize = Math.Max(pageSize, 1);
        TotalOfItems = totalOfItems;
        TotalPages = (int)Math.Ceiling((double)totalOfItems / pageSize);
    }

    public int ItemsToSkip() => Math.Max(PageSize * (CurrentPage - 1), 0);
}