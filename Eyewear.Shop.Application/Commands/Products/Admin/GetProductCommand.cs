using Eyewear.Shop.Application.Dtos.Products;
using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using Eyewear.Shop.Domain.Entities;
using MediatR;

namespace Eyewear.Shop.Application.Commands.Products.Admin
{
    public record GetProductCommand : IRequest<Result<GetProductResponse>>
    {
        public int ProductId { get; set; }
    }

    public record GetProductResponse
    {
        public GetProductDto ProductDto { get; set; }
    }

    #region GetProductDtos
    public record GetProductDto(
    int Id,
    string Name,
    GetProductCategoryDto CategoryDto,
    float? DiscountAmount,
    DiscountType? DiscountTyp,
    decimal BasePrice,
    string Description,
    string ThumbnailUrl,
    List<string> ImageUrls,
    List<GetProductVariantDto> Variants,
    List<GetProductAttributeDto> Attributes
    );

    public record GetProductAttributeDto(
    string Key,
    string Value
    );

    public record GetProductVariantDto(
        int Id,
        string Name,
        decimal Price,
        List<GetProductAttributeDto> Attributes
    );

    public record GetProductCategoryDto(int Id, string Name);

    #endregion


    #region Handler
    public class GetProductCommandHandler : IRequestHandler<GetProductCommand, Result<GetProductResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public GetProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }
        public async Task<Result<GetProductResponse>> Handle(GetProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsyncNoTracking(request.ProductId, cancellationToken);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
            }

            var responseDto = new GetProductDto(
            product.Id,
            product.Name,
            new GetProductCategoryDto(
                product.Category.Id,
                product.Category.Title
            ),
            product.DiscountAmount,
            product.DiscountTyp,
            product.BasePrice,
            product.Description,
            product.ThumbnailImageUrl,
            product.MainImagesUrls as List<string>,
            product.Variants.Select(v => new GetProductVariantDto(
                v.Id,
                v.Name,
                v.VarientPrice,
                v.VarientAttributes.Select(a => new GetProductAttributeDto(
                    a.Key,
                    a.Value
                )).ToList()
            )).ToList(),
            product.Attributes.Select(a => new GetProductAttributeDto(
                a.Key,
                a.Value
            )).ToList()
        );

            var response = new GetProductResponse
            {
                ProductDto = responseDto
            };

            return Result<GetProductResponse>.Success(response);
        }
    } 
    #endregion


}
