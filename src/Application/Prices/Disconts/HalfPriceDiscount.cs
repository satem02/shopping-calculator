using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Products.Disconts
{
    public class HalfPriceDiscount : IDiscount
    {
        public ProductQuantity _productsThatQualifyBasketforDiscount;
        public Product _discountedProduct;

        public HalfPriceDiscount(Product discountProduct , ProductQuantity productsThatQualifyBasketforDiscount)
        {            
            _productsThatQualifyBasketforDiscount = productsThatQualifyBasketforDiscount ?? throw new ArgumentNullException(nameof(productsThatQualifyBasketforDiscount));
            _discountedProduct = discountProduct ?? throw new ArgumentNullException(nameof(discountProduct));
        }
        private decimal ApplyDiscount(Product item) => Math.Round(item.Price * 0.5m, 2);

        public List<AppliedDiscount> DiscountsApplicable(List<ProductQuantity> productList)
        {
            var discountsApplied = new List<AppliedDiscount>();

            foreach (var item in productList)
            {
                if (item.Product.Id == _productsThatQualifyBasketforDiscount.Product.Id && item.Quantity >= _productsThatQualifyBasketforDiscount.Quantity)
                {
                    var halfPriceItems = productList
                        .Where(halfPriceItem => halfPriceItem.Product.Id == _discountedProduct.Id)
                        .ToArray();

                    if (halfPriceItems.Length > 0)
                    {
                        var discount = ApplyDiscount(halfPriceItems[0].Product);

                        discountsApplied.Add(new AppliedDiscount
                        {
                            Type = DiscountType.HalfPrice,
                            Description = $"{_discountedProduct.Name} 50% OFF: - {discount.ToCurrencyString()}",
                            Amount = discount
                        });
                    }
                }
            }

            return discountsApplied;
        }
    }
}
