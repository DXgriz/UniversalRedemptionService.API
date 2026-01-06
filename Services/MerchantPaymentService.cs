using Microsoft.EntityFrameworkCore;
using UniversalRedemptionService.API.Data;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Models;

namespace UniversalRedemptionService.API.Services
{
    public class MerchantPaymentService(SLUCRSDbContext context)
    {
        public async Task PayMerchantAsync(int userId, MerchantPaymentDto dto)
        {
            if (dto.Amount <= 0)
                throw new Exception("Invalid amount");

            using var tx = await context.Database.BeginTransactionAsync();

            var senderWallet = await context.Wallets.Include(w => w.User).FirstAsync(w => w.UserId == userId);

            if (senderWallet.Balance < dto.Amount)
                throw new Exception("Insufficient funds");

            var merchantWallet = await context.Wallets.Include(w => w.User).FirstOrDefaultAsync(w => w.User.Email == dto.MerchantEmail && w.User.IsMerchant);

            if (merchantWallet == null)
                throw new Exception("Merchant not found");

            senderWallet.Balance -= dto.Amount;
            merchantWallet.Balance += dto.Amount;

            context.Transactions.Add(new Transaction
            {
                FromWalletId = senderWallet.Id,
                ToWalletId = merchantWallet.Id,
                Amount = dto.Amount,
                Type = TransactionType.Payment,
                Reference = dto.Reference
            });

            await context.SaveChangesAsync();
            await tx.CommitAsync();
        }
    }
}
