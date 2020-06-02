using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IProductService
    {
        Product GetProductById(int id);
        
        Product GetProductByName(string name);

        List<Product> GetProducts();

        Task<List<IDiscount>> GetProductDiscounts();

    }
}