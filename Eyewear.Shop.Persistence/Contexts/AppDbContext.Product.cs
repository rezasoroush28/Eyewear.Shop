using Eyewear.Shop.Application.Interfaces.Persistance.Repository;
using Eyewear.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Eyewear.Shop.Persistence.Contexts;

public partial class AppDbContext : IProductRepository
{
    public async Task AddAsync(Product product)
    {
        await Products.AddAsync(product);
    }

    public async void Delete(int id)
    {
        var product = await Products.Where(p => p.Id == id).FirstOrDefaultAsync();
        product.IsDeleted = true;

    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await Products.AsNoTracking().Where(p => p.Id == id && !p.IsDeleted ).FirstOrDefaultAsync();
    }
}

