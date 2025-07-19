namespace WebUIOver.Shared.Models;

public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Data { get; }

    public int Page { get; }

    public int PerPage { get; }

    public int TotalPages { get; }

    public int TotalCount { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        Page = pageNumber;
        PerPage = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Data = items;
    }

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;
}

