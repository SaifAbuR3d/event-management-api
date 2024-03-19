namespace EventManagement.Application.Contracts.Responses;

public class PaginationMetadata(int pageNumber, int pageSize, int totalCount)
{
    public int PageIndex { get; private set; } = pageNumber;
    public int PageSize { get; private set; } = pageSize;
    public int TotalCount { get; private set; } = totalCount;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
    public string? PreviousPageLink { get; set; }
    public string? NextPageLink { get; set; }
}
