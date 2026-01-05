using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Services;

namespace UniversalRedemptionService.API.Controllers
{
    [ApiController]
    [Route("api/redemptions")]
    [Authorize]
    public class RedemptionController(RedemptionService service) : ControllerBase
    {
        [HttpPost("redeem")]
        public async Task<IActionResult> Redeem(RedeemCashSendDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")!.Value);

            await service.RedeemAsync(userId, dto);

            return Ok(new
            {
                message = "Redeemed successfully"
            });
        }
    }
}

