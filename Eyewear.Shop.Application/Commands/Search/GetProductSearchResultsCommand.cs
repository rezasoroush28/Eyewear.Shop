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

public class GetProductSearchResultsCommand : IRequest<Result<GetProductSearchResultsResponse>>
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


public class GetProductSearchResultsResponse
{
    public List<ProductSearchReult> SearchProductRsults { get; set; }
}

#region Dtos
public struct ProductSearchReult(string ProductyName, string ThumbnailUrl, decimal Price);

#endregion

#region Handler

public class GetProductSearchResultsHandler : IRequestHandler<GetProductSearchResultsCommand, Result<GetProductSearchResultsResponse>>
{
    private readonly ISearchservice _searchservice;
    private readonly IProductRepository _productRepository;
    public async Task<Result<GetProductSearchResultsResponse>> Handle(GetProductSearchResultsCommand request, CancellationToken cancellationToken)
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
            return Result<GetProductSearchResultsResponse>.Failure(searchRes.ErrorMessage);
        }

        if (searchRes.Data.Count > 1)
        {
            await _searchservice.AddSearchQueryToPopularSearches(request.StringQuery, cancellationToken);
        }

        var productResults = _productRepository.GetProductsByIds(searchRes.Data
            .Select(x => x.Id).ToList(), false, false, false, cancellationToken)
            .Result.Select(p => new ProductSearchReult(p.Name, p.ThumbnailImageUrl, p.BasePrice)).ToList();

        return Result<GetProductSearchResultsResponse>.Success(new GetProductSearchResultsResponse
        {
            SearchProductRsults = productResults
        });
    }



}

#endregion
