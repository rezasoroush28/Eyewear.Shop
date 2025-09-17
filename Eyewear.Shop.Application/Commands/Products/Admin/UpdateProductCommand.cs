using Eyewear.Shop.Application.Commands.Products.Admin;
using Eyewear.Shop.Application.Dtos.Products;
using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using Eyewear.Shop.Domain.Entities;
using MediatR;

namespace Eyewear.Shop.Application.Commands.Products.Admin
{
    public record UpdateProductCommand : IRequest<Result<UpdateProductResponse>>
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public float? DiscountAmount { get; set; }
        public DiscountType? DiscountTyp { get; set; }
        public decimal BasePrice { get; set; }
        public List<ProductAttributeDto> AttributeList { get; set; } = new List<ProductAttributeDto>();
        public List<string> MainImagesUrls { get; set; } = new List<string>();
        public string ThumbnailImageUrl { get; set; } = string.Empty;
        public List<ProductVariantDto> VarientList { get; set; } = new List<ProductVariantDto>();
    }
}

public record UpdateProductResponse { }


public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<UpdateProductResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<Result<UpdateProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsyncTracking(request.ProductId, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
        }

        var attributes = (request.AttributeList ?? new()).Select(a => new ProductAttribute
        {
            Key = a.Key,
            Value = a.Value
        }).ToList();

        var variants = (request.VarientList ?? new()).Select(v => new ProductVariant
        {
            // No ProductId here; EF will set it via navigation
            VarientPrice = v.VarientPrice,
            VarientAttributes = (v.VarientAttributes ?? new()).Select(va => new ProductAttribute
            {
                Key = va.Key,
                Value = va.Value
            }).ToList()
        }).ToList();

        product.Name = request.Name;
        product.Description = request.Description;
        product.CategoryId = request.CategoryId;
        product.BasePrice = request.BasePrice;
        product.DiscountAmount = request.DiscountAmount;
        product.DiscountTyp = request.DiscountTyp;
        product.Attributes = attributes;
        product.ThumbnailImageUrl = request.ThumbnailImageUrl;
        product.ThumbnailImageUrl = request.ThumbnailImageUrl;
        product.Variants = variants;



        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UpdateProductResponse>.Success(new UpdateProductResponse());
    }
}


