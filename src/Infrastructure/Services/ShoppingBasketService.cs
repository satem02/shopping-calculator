using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class ShoppingBasketService : IShoppingBasketService
    {
        private readonly IProductService _productService;

        public ShoppingBasketService(IProductService productService)
        {
            _productService = productService;
        }

        public List<ProductQuantity> AddProducts(string[] productNames)
        {
            var result = new List<ProductQuantity>();

            if(productNames is null)
            {
                throw new NullReferenceException($"Product name can't be empty");
            }

            foreach (var productName in productNames)
            {
                var product = _productService.GetProductByName(productName);
                
                if(product == null)
                {
                    throw new NotSupportedException($"Unrecognized product name : {productName.ToLower()}");
                }

                var existProduct = result.SingleOrDefault(item => item.Product.Id == product.Id);

                if (existProduct == null)
                {
                    result.Add(new ProductQuantity
                    {
                        Product = product,
                        Quantity = 1
                    });
                }

                if (existProduct != null)
                    existProduct.Quantity += 1;
            }

            return result;
        }
    }
}
