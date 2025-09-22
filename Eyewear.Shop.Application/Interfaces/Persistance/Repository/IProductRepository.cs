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
        #region Admin
        Task<int> AdminGetTotalProductsCount(CancellationToken cancellationToken);
        Task AdminAddAsync(Product product, CancellationToken cancellationToken);
        Task AdminUpdate(Product product);
        Task<Product> AdminGetByIdNoTrackingAsync(int id, CancellationToken cancellationToken);
        Task<Product> AdminGetByIdTrackingAsync(int id, CancellationToken cancellationToken);
        Task<List<Product>> AdminGetAllWithPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task AdminDeleteAsync(int id);
        #endregion

        Task<List<Product>> GetAllProductsAsync(
                            bool includeCategory = false,
                            bool includeVariants = false,
                            bool includeAttributes = false,
                            CancellationToken cancellationToken = default);
    }
}
