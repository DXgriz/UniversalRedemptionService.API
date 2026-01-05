using UniversalRedemptionService.API.Models;

namespace UniversalRedemptionService.API.Data
{
    public class DbSeeder(SLUCRSDbContext context)
    {
        public async Task SeedAsync()
        {
            if (!context.CashSendRedemptions.Any())
            {
                context.CashSendRedemptions.AddRange(
                    new CashSendRedemption
                    {
                        Provider = "eWallet",
                        ReferenceNumber = "EW123456",
                        Amount = 500
                    },
                    new CashSendRedemption
                    {
                        Provider = "CashSend",
                        ReferenceNumber = "CS789012",
                        Amount = 1000
                    }
                );
                await context.SaveChangesAsync();
            }
        }


    }
}
