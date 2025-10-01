using Eyewear.Shop.Application.Events;
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
        private readonly IMediator _mediator;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        #region Handler
        public async Task<Result<DeleteProductResponse>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.AdminGetByIdTrackingAsync(request.ProductId, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");
            }

            await _productRepository.AdminDeleteAsync(request.ProductId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(new ProductDeletedEvent(product));

            return Result<DeleteProductResponse>.Success(new DeleteProductResponse());
        }

        #endregion   
    }
}


    
