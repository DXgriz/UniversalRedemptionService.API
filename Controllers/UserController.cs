using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversalRedemptionService.API.Services;


namespace UniversalRedemptionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController(UserService service) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await service.GetProfileAsync(userId);
            return Ok(profile);
        }
    }
}
