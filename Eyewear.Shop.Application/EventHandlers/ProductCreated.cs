using Eyewear.Shop.Application.Commands.Search;
using Eyewear.Shop.Application.Interfaces.Services;
using Eyewear.Shop.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Events
{
    public class UpdateIndexEventHandler :
        INotificationHandler<ProductCreatedEvent>,
        INotificationHandler<ProductUpdatedEvent>,
        INotificationHandler<ProductDeletedEvent>
    {

        private readonly IMediator _mediator;

        public UpdateIndexEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await UpdateIndex();
        }

        public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
        {
            await UpdateIndex();
        }

        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            await UpdateIndex();
        }

        private async Task UpdateIndex()
        {
            await _mediator.Send(new IndexAllProductCommand());
        }
    }
}
