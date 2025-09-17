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
        Task AddAsync(Product product, CancellationToken cancellationToken);
        Task Update(Product product);
        Task<Product> GetByIdAsyncNoTracking(int id, CancellationToken cancellationToken)
        Task<Product> GetByIdAsyncTracking(int id, CancellationToken cancellationToken);
        Task<List<Product>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
