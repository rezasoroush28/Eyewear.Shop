using Eyewear.Shop.Application.Dtos.Products;
using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using Eyewear.Shop.Domain.Entities;
using MediatR;

namespace Eyewear.Shop.Application.Commands.Products.Admin
{
    public record GetAllProductsCommand : IRequest<Result<GetAllProductsResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public record GetAllProductsResponse
    {
        public List<GetAllProductDto> AllProductDtos { get; set; }
    }

    #region GetProductDtos

    public record GetAllProductDto(int Id, string Name, decimal BasePrice, string ThumbnailUrl);
    #endregion


    #region Handler
    public class GetAllProductsCommandHandler : IRequestHandler<GetAllProductsCommand, Result<GetAllProductsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public GetAllProductsCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }
        public async Task<Result<GetAllProductsResponse>> Handle(GetAllProductsCommand request, CancellationToken cancellationToken)
        {
            var query = await _productRepository.AdminGetAllWithPaginationAsync(request.PageNumber, request.PageSize
                , cancellationToken);
            var resultDtos = query.Select(p => new GetAllProductDto(
                p.Id,
                p.Name,
                p.BasePrice,
                p.ThumbnailImageUrl
                )).ToList();

            return Result<GetAllProductsResponse>.Success(new GetAllProductsResponse { AllProductDtos = resultDtos });
        }
    } 
    #endregion


}
