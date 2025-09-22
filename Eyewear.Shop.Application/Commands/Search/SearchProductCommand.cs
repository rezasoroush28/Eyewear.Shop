using Eyewear.Shop.Application.Commands.Products.Admin;
using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
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
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}


public class SearchProductResponse
{
    List<ProductSearchDto> SearchedProducts { get; set; }
}

#region Dtos
public struct ProductSearchReult(string ProductyName, string ThumbnailUrl, double Price);

#endregion

#region Handler

public class SearchProductCommandHandler : IRequestHandler<SearchProductCommand, Result<SearchProductResponse>>
{
    public async Task<Result<SearchProductResponse>> Handle(SearchProductCommand request, CancellationToken cancellationToken)
    {

    }
}

#endregion
