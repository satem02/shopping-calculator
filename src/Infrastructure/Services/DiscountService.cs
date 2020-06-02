using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly string _noOfferMessage = "(No offers available)";
        public List<AppliedDiscount> CalculateBasketDiscounts(List<ProductQuantity> basket, List<IDiscount> discounts)
        {
            if(basket is null || discounts is null)
            {
                throw new NullReferenceException($"basket or discounts can't be empty");
            }
            
            var basketDiscounts = new List<AppliedDiscount>();

            foreach (var discount in discounts)
            {
                basketDiscounts.AddRange(discount.DiscountsApplicable(basket));
            }

            if (!basketDiscounts.Any())
            {
                basketDiscounts.Add(new AppliedDiscount
                {
                    Type = DiscountType.None,
                    Description = _noOfferMessage ,
                    Amount = 0.00m
                });           
            }

            return basketDiscounts;
        }

    }
}