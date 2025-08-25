using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eyewear.Shop.Domain.Entities;

public class Category
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int? ParentCategoryId { get; set; }

    public virtual Category? ParentCategory { get; set; }

    public virtual ICollection<Category> Subcategories { get; set; } = new List<Category>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
