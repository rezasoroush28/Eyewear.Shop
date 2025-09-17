using Eyewear.Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Interfaces.Persistance.Repository
{
    public interface IProductRepository
    {
        Task<int> AdminGetTotalProductsCount(CancellationToken cancellationToken);
        Task AdminAddAsync(Product product, CancellationToken cancellationToken);
        Task AdminUpdate(Product product);
        Task<Product> AdminGetByIdAsyncNoTracking(int id, CancellationToken cancellationToken);
        Task<Product> AdminGetByIdAsyncTracking(int id, CancellationToken cancellationToken);
        Task<List<Product>> AdminGetAllAsyncWithPagination(int pageNumber,int pageSize,CancellationToken cancellationToken);
        Task AdminDeleteAsync(int id);
    }
}
