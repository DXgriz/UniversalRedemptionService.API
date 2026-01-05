using Microsoft.EntityFrameworkCore;
using UniversalRedemptionService.API.Data;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Models;

namespace UniversalRedemptionService.API.Services
{
    public class WalletService(SLUCRSDbContext context)
    {
        public async Task<decimal> GetBalanceAsync(int userId)
        {
            var wallet = await context.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.UserId == userId);

            return wallet == null ? throw new Exception("Wallet not found") : wallet.Balance;
        }

        public async Task TransferAsync(int senderUserId, TransferRequestDto dto)
        {
            if (dto.Amount <= 0)
                throw new Exception("Invalid amount");

            //Idempotency pre-check
            var existingTransaction = await context.Transactions.AsNoTracking().FirstOrDefaultAsync(t => t.Reference == dto.Reference);

            if (existingTransaction != null)
                return;

            using var dbTransaction = await context.Database.BeginTransactionAsync();

            try
            {
                var senderWallet = await context.Wallets.Include(w => w.User).FirstOrDefaultAsync(w => w.UserId == senderUserId)?? throw new Exception("Sender wallet not found");

                if (senderWallet.Balance < dto.Amount)
                    throw new Exception("Insufficient funds");

                var recipientWallet = await context.Wallets.Include(w => w.User).FirstOrDefaultAsync(w => w.User.Email == dto.RecipientEmail)?? throw new Exception("Recipient not found");

                if (senderWallet.Id == recipientWallet.Id)
                    throw new Exception("Cannot transfer to yourself");

                // Update balances
                senderWallet.Balance -= dto.Amount;
                recipientWallet.Balance += dto.Amount;

                // Ledger entry
                context.Transactions.Add(new Transaction
                {
                    FromWalletId = senderWallet.Id,
                    ToWalletId = recipientWallet.Id,
                    Amount = dto.Amount,
                    Type = TransactionType.Transfer,
                    Reference = dto.Reference,
                    CreatedAt = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Transactions_Reference") == true)
            {
                // Duplicate reference → idempotent success
                await dbTransaction.RollbackAsync();
            }
        }

    }
}
