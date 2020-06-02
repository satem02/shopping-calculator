
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IDiscountService
    {
        List<AppliedDiscount> CalculateBasketDiscounts(List<ProductQuantity> basket, List<IDiscount> Discounts);
    }
}