using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Products.Disconts;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        public ProductService()
        {
        }
        public List<Product> GetProducts()
        {
            var result = new List<Product>();

            var product1 = new Product()
            {
                Id = 1,
                Name = "Beans",
                Price = 65
            };
            result.Add(product1);

            var product2 = new Product()
            {
                Id = 2,
                Name = "Bread",
                Price = 80
            };
            result.Add(product2);

            var product3 = new Product()
            {
                Id = 3,
                Name = "Milk",
                Price = 130
            };
            result.Add(product3);

            var product4 = new Product()
            {
                Id = 4,
                Name = "Apples",
                Price = 100
            };
            result.Add(product4);

            return result;
        }

        public Product GetProductById(int id)
        {
            return GetProducts().FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<IDiscount>> GetProductDiscounts()
        {
            var result = new List<IDiscount>();
            
            await Task.Delay(2);
            
            var productDiscont1 = new PercentageDiscount(GetProductById(4), 0.10m);
            result.Add(productDiscont1);


            var productDiscont2 = new HalfPriceDiscount(GetProductById(2), new ProductQuantity()
            {
                Product = GetProductById(1),
                Quantity = 2
            });
            result.Add(productDiscont2);

            return result;
        }

        public Product GetProductByName(string name)
        {
            var product = GetProducts().SingleOrDefault(item =>
                    string.Equals(item.Name, name, StringComparison.CurrentCultureIgnoreCase));

            return product;
        }
    }
}
