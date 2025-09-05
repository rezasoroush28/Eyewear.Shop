public class SearchRequestDto
{
    public string Query { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public string? SortBy { get; set; } // "price", "createdAt", "popularity"
    public string? SortOrder { get; set; } // "asc" or "desc"
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? CategoryId { get; set; }
}
