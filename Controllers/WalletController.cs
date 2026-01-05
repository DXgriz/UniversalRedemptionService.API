using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Services;

namespace UniversalRedemptionService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController(WalletService walletService) : ControllerBase
    {

        //Helper to get logged-in user ID
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("Invalid token");

            return int.Parse(userIdClaim);
        }

        //GET: api/wallet/balance
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = GetUserId();
            var balance = await walletService.GetBalanceAsync(userId);

            return Ok(new
            {
                balance
            });
        }

        //POST: api/wallet/transfer
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();

            await walletService.TransferAsync(userId, dto);

            return Ok(new
            {
                message = "Transfer processed successfully",
                reference = dto.Reference
            });
        }
    }
}
