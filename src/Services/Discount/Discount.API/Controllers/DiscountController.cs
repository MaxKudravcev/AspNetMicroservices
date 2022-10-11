using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _repository;

        public DiscountController(IDiscountRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), 200)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName) =>
            Ok(await _repository.GetDiscount(productName));

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), 200)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            await _repository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), 200)]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon) =>
            Ok(await _repository.UpdateDiscount(coupon));

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName) =>
            Ok(await _repository.DeleteDiscount(productName));
    }
}
