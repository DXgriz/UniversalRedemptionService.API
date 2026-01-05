using Microsoft.EntityFrameworkCore;
using UniversalRedemptionService.API.Data;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Models;
using UniversalRedemptionService.API.Providers;

namespace UniversalRedemptionService.API.Services
{
    public class RedemptionService(
        SLUCRSDbContext context,
        ICashSendProvider provider)
    {
        public async Task RedeemAsync(int userId, RedeemCashSendDto dto)
        {
            //Validate with external provider FIRST
            var validation = await provider.ValidateAsync(dto.ReferenceNumber, dto.Pin);

            if (!validation.IsValid)
                throw new Exception("Invalid or expired!");

            //Prevent double redemption (DB-level protection)
            var alreadyRedeemed = await context.CashSendRedemptions.AnyAsync(x => x.ReferenceNumber == dto.ReferenceNumber&& x.IsRedeemed);

            if (alreadyRedeemed)
                throw new Exception("Already redeemed!");

            using var tx = await context.Database.BeginTransactionAsync();

            //Get wallet
            var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId)?? throw new Exception("Wallet not found");

            //Persist redemption (ONCE)
            var redemption = new CashSendRedemption
            {
                Provider = validation.ProviderName,
                ReferenceNumber = dto.ReferenceNumber,
                PinHash = BCrypt.Net.BCrypt.HashPassword(dto.Pin),
                Amount = validation.Amount,
                IsRedeemed = true,
                RedeemedAt = DateTime.UtcNow,
                WalletId = wallet.Id
            };

            context.CashSendRedemptions.Add(redemption);

            //Credit wallet using provider amount
            wallet.Balance += validation.Amount;

            //Log transaction
            context.Transactions.Add(new Transaction
            {
                FromWalletId = null,
                ToWalletId = wallet.Id,
                Amount = validation.Amount,
                Type = TransactionType.Redemption,
                Reference = dto.ReferenceNumber
            });

            await context.SaveChangesAsync();
            await tx.CommitAsync();
        }
    }
}
