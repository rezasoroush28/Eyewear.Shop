using Eyewear.Shop.Domain.Entities;
using MediatR;

namespace Eyewear.Shop.Application.Events
{
    public record ProductDeletedEvent(Product Product) : INotification;

}
