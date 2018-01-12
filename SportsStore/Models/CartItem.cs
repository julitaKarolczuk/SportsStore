using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.Models
{
    public class CartItem
    {
        public ProductItem Product { get; set; }
        public int Count { get; set; }

        public CartItem() { }

        public CartItem(Product product)
        {
            Product = new ProductItem(product);
            Count = 1;
        }
    }

    public class ProductItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public bool Hidden { get; set; }
        public string Producer { get; set; }
        public string Description { get; set; }

        public ProductItem() { }

        public ProductItem(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            CategoryId = product.CategoryId;
            Price = product.Price;
            Picture = product.Picture;
            Hidden = product.Hidden;
            Producer = product.Producer;
            Description = product.ShortDescription;
        }
    }
}