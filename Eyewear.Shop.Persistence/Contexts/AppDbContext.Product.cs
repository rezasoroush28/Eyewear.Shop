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
    public async Task AdminAddAsync(Product product, CancellationToken cancellationToken)
    {
        await Products.AddAsync(product, cancellationToken);
    }

    public async Task AdminDeleteAsync(int id)
    {
        var product = await Products.Where(p => p.Id == id).FirstOrDefaultAsync();
        product.IsDeleted = true;

    }
    public async Task<List<Product>> AdminGetAllAsyncWithPagination(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await Products.AsNoTracking()
            .OrderByDescending(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product> AdminGetByIdAsyncTracking(int id, CancellationToken cancellationToken)
    {
        return await Products.Where(p => p.Id == id && !p.IsDeleted).FirstOrDefaultAsync();
    }

    public async Task<Product> AdminGetByIdAsyncNoTracking(int id, CancellationToken cancellationToken)
    {
        return await Products.AsNoTracking().Where(p => p.Id == id && !p.IsDeleted).FirstOrDefaultAsync();
    }

    public async Task AdminUpdate(Product product)
    {
       Products.Update(product);
    }

    public async Task<int> AdminGetTotalProductsCount(CancellationToken cancellationToken)
    {
        return await Products.CountAsync(cancellationToken);
    }
}

