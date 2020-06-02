using Application.Common.Interfaces;
using Moq;

namespace Application.XUnit.Base
{
    public class TestBase
    {
        protected readonly Mock<IShoppingBasketService> _shoppingBasketService;
        protected readonly Mock<IProductService> _productService;
        protected readonly Mock<IDiscountService> _discountService;

        public TestBase()
        {
            _productService = new Mock<IProductService>();
            _shoppingBasketService = new Mock<IShoppingBasketService>();
            _discountService = new Mock<IDiscountService>();
        }
    }
}