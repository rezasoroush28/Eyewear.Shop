using Eyewear.Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyewear.Shop.Application.Dtos.Products
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public float? DiscountAmount { get; set; }
        public DiscountType? DiscountTyp { get; set; }
        public decimal BasePrice { get; set; }
        public List<ProductAttributeDto> AttributeList { get; set; } = new List<ProductAttributeDto>();
        public List<string> MainImagesUrls { get; set; } = new List<string>();
        public string ThumbnailImageUrl { get; set; } = string.Empty;
        public List<ProductVariantDto> VarientList { get; set; } = new List<ProductVariantDto>();
    }

    public class ProductAttributeDto
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }

    public class ProductImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
    public class ProductVariantDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public decimal VarientPrice { get; set; }
        public List<ProductAttributeDto> VarientAttributes { get; set; } = new();


    }
}
