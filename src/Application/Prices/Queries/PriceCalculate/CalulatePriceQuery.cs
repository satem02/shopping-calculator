using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Prices.Queries.PriceCalculate
{
    public class CalulatePriceQuery : IRequest<PriceCalculateVm>
    {
        public string[] Products { get; set; }
    }

    public class GetDummyProductsHandler : IRequestHandler<CalulatePriceQuery, PriceCalculateVm>
    {

        private readonly IProductService _productService;
        private readonly IShoppingBasketService _shoppingBasketService;
        private readonly IDiscountService _discountService;

        public GetDummyProductsHandler(IProductService productService, IShoppingBasketService shoppingBasketService,IDiscountService discountService)
        {
            _shoppingBasketService = shoppingBasketService;
            _productService = productService;
            _discountService = discountService;
        }

        public async Task<PriceCalculateVm> Handle(CalulatePriceQuery request, CancellationToken cancellationToken)
        {
            var result = new PriceCalculateVm();
            
            var basket = _shoppingBasketService.AddProducts(request.Products);
            var discounts =  await _productService.GetProductDiscounts();

            var applyDiscount = _discountService.CalculateBasketDiscounts(basket,discounts);
            
            var subTotalPrice = basket.Sum(item => item.Product.Price * item.Quantity);
            
            var totalPrice = subTotalPrice - applyDiscount.Sum(item => item.Amount);


            result.PriceInfo = new PriceInfoDto() {
                Total = totalPrice,
                SubTotal =subTotalPrice,
                DiscontMessage = string.Join(" --- ", applyDiscount.Select(x => x.Description))
            };

            return result;
        }

    }
}
