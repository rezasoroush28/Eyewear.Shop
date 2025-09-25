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
    public async Task<List<Product>> AdminGetAllWithPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await Products.AsNoTracking()
            .OrderByDescending(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product> AdminGetByIdTrackingAsync(int id, CancellationToken cancellationToken)
    {
        return await Products.Where(p => p.Id == id && !p.IsDeleted).FirstOrDefaultAsync();
    }

    public async Task<Product> AdminGetByIdNoTrackingAsync(int id, CancellationToken cancellationToken)
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

    public async Task<List<Product>> GetAllProductsAsync(
    bool includeCategory = false,
    bool includeVariants = false,
    bool includeAttributes = false,
    CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = Products.AsNoTracking().AsSplitQuery();

        if (includeCategory)
            query = query.Include(p => p.Category);

        if (includeVariants)
            query = query.Include(p => p.Variants);

        if (includeAttributes)
            query = query.Include(p => p.Attributes);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<Product>> GetProductsByIds(List<int> ids, bool includeCategory = false, bool includeVariants = false, bool includeAttributes = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = Products
            .AsNoTracking()
            .AsSplitQuery()
            .Where(p => ids.Contains(p.Id));

        if (includeCategory)
            query = query.Include(p => p.Category);

        if (includeVariants)
            query = query.Include(p => p.Variants);

        if (includeAttributes)
            query = query.Include(p => p.Attributes);

        return await query.ToListAsync(cancellationToken);
    }
}

