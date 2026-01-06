using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Services;

namespace UniversalRedemptionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/payments")]
    public class MerchantPaymentController(MerchantPaymentService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Pay(MerchantPaymentDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await service.PayMerchantAsync(userId, dto);
            return Ok("Payment successful");
        }

    }
}
