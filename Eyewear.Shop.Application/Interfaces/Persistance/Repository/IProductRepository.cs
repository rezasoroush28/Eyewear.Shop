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
        Task AddAsync(Product product);
        Task<Product> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        void Delete(int id);
    }
}
