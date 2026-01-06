using Microsoft.EntityFrameworkCore;
using UniversalRedemptionService.API.Data;
using UniversalRedemptionService.API.DTOs;

namespace UniversalRedemptionService.API.Services
{
    public class UserService(SLUCRSDbContext context)
    {
        public async Task<UserProfileDto> GetProfileAsync(int userId)
        {
            return await context.Users.Where(u => u.Id == userId)
                .Select(u => new UserProfileDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Balance = u.Wallet.Balance,
                    IsMerchant = u.IsMerchant
                })
                .FirstAsync();
        }
    }
}
