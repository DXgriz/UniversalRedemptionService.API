using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Services;

namespace UniversalRedemptionService.API.Controllers
{
    [EnableRateLimiting("user-policy")]
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RedemptionController(RedemptionService service) : ControllerBase
    {
        [HttpPost("redeem")]
        public async Task<IActionResult> Redeem([FromBody] RedeemCashSendDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var userId = int.Parse(User.FindFirst("userId")!.Value);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await service.RedeemAsync(userId, dto);

            return Ok(new
            {
                message = "Redeemed successfully"
            });
        }
    }
}

