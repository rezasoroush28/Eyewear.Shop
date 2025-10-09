using Eyewear.Shop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eyewear.Shop.Persistence.Contexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<OtpCode> OtpCodes => Set<OtpCode>();   
    //public DbSet<Discount> Discounts => Set<Discount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
