using Eyewear.Shop.Application.Commands.Products.Admin;
using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using Eyewear.Shop.Application.Interfaces.Services;
using Eyewear.Shop.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Eyewear.Shop.Application.Commands.Search;

public class IndexAllProductCommand : IRequest<Result<IndexAllProductResponse>>
{

}


public class IndexAllProductResponse
{

}

#region Handler

public class IndexAllProductCommandHandler : IRequestHandler<IndexAllProductCommand, Result<IndexAllProductResponse>>
{

    private readonly ISearchservice _searchservice;
    private readonly IProductRepository _productRepository;
    public async Task<Result<IndexAllProductResponse>> Handle(IndexAllProductCommand request, CancellationToken cancellationToken)
    {
        var products = _productRepository.GetAllProductsAsync(includeCategory: true, false, false, cancellationToken)
             .Result.Select(p => new ProductSearchDto
             {
                 Id = p.Id,
                 Name = p.Name,
                 Category = p.Category?.Title ?? string.Empty,
                 ParentCategory = p.Category?.ParentCategory?.Title ?? string.Empty,
                 Description = p.Description,
                 Attributes = p.Attributes.Select(a => a.Value).ToList()
             }).ToList();

        await _searchservice.IndexAllProduct(products, cancellationToken);
        return Result<IndexAllProductResponse>.Success(new IndexAllProductResponse());

    }
}

#endregion
