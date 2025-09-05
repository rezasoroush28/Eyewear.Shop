﻿namespace Eyewear.Shop.API.Models.Search
{
    public class SearchProductRequestModel
    {
        public string? StringQuery {  get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        // Sort options: "newest", "priceAsc", "priceDesc"
        public string? SortBy { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
