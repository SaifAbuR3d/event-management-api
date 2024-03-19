namespace EventManagement.Application.Contracts.Requests;

public class GetAllQueryParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    public int PageIndex { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    /// <summary>
    /// search by:
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// sort by:
    /// </summary>
    public string? SortColumn { get; set; }

    /// <summary>
    /// asc or desc
    /// </summary>
    public string? SortOrder { get; set; }
}
