using Eyewear.Shop.Application.Dtos.Products;
using Eyewear.Shop.Application.Events;
using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using Eyewear.Shop.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Commands.Products.Admin
{
    public class CreateProductCommand : IRequest<Result<CreateProductResponse>>
    {
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

    public class CreateProductResponse
    {

    }

    #region Handler

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<CreateProductResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
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

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                BasePrice = request.BasePrice,
                DiscountAmount = request.DiscountAmount,
                DiscountTyp = request.DiscountTyp,
                Attributes = attributes,
                MainImagesUrls = request.MainImagesUrls,
                ThumbnailImageUrl = request.ThumbnailImageUrl,
                Variants = variants

            };

            await _productRepository.AdminAddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(new ProductCreatedEvent(product));

            return Result<CreateProductResponse>.Success(new CreateProductResponse());

        }
    } 
    #endregion

}
