using System.Threading.Tasks;
using Application.Prices.Queries.PriceCalculate;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class PriceController: ApiController
    {
        [HttpPost]
        public async Task<ActionResult<PriceCalculateVm>> Create(CalulatePriceQuery command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
