using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversalRedemptionService.API.Services;

namespace UniversalRedemptionService.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/stats")]
    public class AdminStatsController(TransactionStatsService service) : ControllerBase
    {
        [HttpGet("transactions")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await service.GetStatsAsync();
            return Ok(stats);
        }
    }
}
