using Eyewear.Shop.Application.Commands.Products.Admin;
using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using Eyewear.Shop.Application.Interfaces.Services;
using Eyewear.Shop.Application.Models.Search;
using Eyewear.Shop.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Commands.Search;

public class SearchProductCommand : IRequest<Result<SearchProductResponse>>
{
    public string? StringQuery { get; set; }
    public string CategoryString { get; set; }  = string.Empty;
    public int? CategoryId { get; set; }
    //public decimal? MinPrice { get; set; }
    //public decimal? MaxPrice { get; set; }
    public string SortBy { get; set; } = "relevance";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}


public class SearchProductResponse
{
    public List<ProductSearchResultModel> SearchedProducts { get; set; }
}



#region Handler

public class SearchProductCommandHandler : IRequestHandler<SearchProductCommand, Result<SearchProductResponse>>
{
    private readonly ISearchservice _searchservice;
    public async Task<Result<SearchProductResponse>> Handle(SearchProductCommand request, CancellationToken cancellationToken)
    {
        var searchRes = await _searchservice.GetSearchResponse(new SearchRequestDto
        {
            StringQuery = request.StringQuery,
            PageSize = request.PageSize,
            Page = request.Page,
            CategoryString = request.CategoryString,
            SortBy = request.SortBy,
        }, cancellationToken);

        if (!searchRes.IsSuccess)
        {
            return Result<SearchProductResponse>.Failure(searchRes.ErrorMessage);
        }



        return Result<SearchProductResponse>.Success(new SearchProductResponse
        {
            SearchedProducts = searchRes.Data
        }
        );

    }

    
}
#endregion