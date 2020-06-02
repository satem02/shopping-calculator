
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IDiscount
    {
        List<AppliedDiscount> DiscountsApplicable(List<ProductQuantity> items);
    }
}