using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using MediatR;

namespace Eyewear.Shop.Application.Commands.Products.Admin
{
    public record DeleteProductCommand : IRequest<Result<DeleteProductResponse>>
    {
        public int ProductId { get; set; }
    }

    public record DeleteProductResponse { }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<DeleteProductResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        #region Handler
        public async Task<Result<DeleteProductResponse>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.AdminGetByIdAsyncTracking(request.ProductId, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
            }

            await _productRepository.AdminDeleteAsync(request.ProductId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<DeleteProductResponse>.Success(new DeleteProductResponse());
        }

        #endregion   
    }
}


    
