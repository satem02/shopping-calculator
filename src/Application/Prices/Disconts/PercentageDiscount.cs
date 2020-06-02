using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Products.Disconts
{    public class PercentageDiscount : IDiscount
    {
        public  Product _discountedProduct;
        public  decimal _percentage;

        public PercentageDiscount(Product discountedProduct , decimal percentage)
        {
            _discountedProduct = discountedProduct ?? throw new ArgumentNullException(nameof(discountedProduct));
            _percentage = percentage;
        }

        private decimal CalculateDiscount(ProductQuantity item) => Math.Round((item.Product.Price * item.Quantity) * _percentage, 2);

        public List<AppliedDiscount> DiscountsApplicable(List<ProductQuantity> productList)
        {
            var discountsApplied = new List<AppliedDiscount>();

            foreach (var item in productList)
            {               
                if (item.Product.Id == _discountedProduct.Id)
                {
                    var discount = CalculateDiscount(item);
                    var appliedDiscount = new AppliedDiscount
                    {
                        Type = DiscountType.Percentage,
                        Description = $"{item.Product.Name} {_percentage:P0} OFF: - {discount.ToCurrencyString()}",
                        Amount = discount
                    };

                    discountsApplied.Add(appliedDiscount);
                }                
            }

            return discountsApplied;
        }
    }
}
