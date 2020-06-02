using System.Collections.Generic;
using Domain.Entities;

namespace Application.Common.Interfaces
{    
    public interface IShoppingBasketService
    {
        List<ProductQuantity> AddProducts(string[] productNames);
    }
}
