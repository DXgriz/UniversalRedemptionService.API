using Microsoft.EntityFrameworkCore;
using UniversalRedemptionService.API.Data;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Models;

namespace UniversalRedemptionService.API.Services
{
    public class TransactionService(SLUCRSDbContext context)
    {

        /// <summary>
        /// User-only paged transactions (delegates to Query)
        /// </summary>
        public async Task<PagedResult<TransactionDto>> GetUserTransactionsAsync(int userId, int page, int pageSize)
        {
            var walletId = await context.Wallets.Where(w => w.UserId == userId).Select(w => w.Id).FirstOrDefaultAsync();

            if (walletId == 0)
                throw new Exception("Wallet not found");

            return await QueryTransactionsAsync(walletId, new TransactionQueryDto
            {
                Page = page,
                PageSize = pageSize
            });
        }

        /// <summary>
        /// Admin / User transaction query
        /// walletId = null → Admin
        /// walletId != null → User
        /// </summary>
        public async Task<PagedResult<TransactionDto>> QueryTransactionsAsync(int? walletId, TransactionQueryDto queryDto)
        {
            var query = context.Transactions.AsNoTracking().AsQueryable();

            //Wallet scope
            if (walletId.HasValue)
            {
                query = query.Where(t => t.FromWalletId == walletId || t.ToWalletId == walletId);
            }

            //Date filters
            if (queryDto.FromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= queryDto.FromDate);

            if (queryDto.ToDate.HasValue)
                query = query.Where(t => t.CreatedAt <= queryDto.ToDate);

            //Type filter
            if (queryDto.Type.HasValue)
                query = query.Where(t => t.Type == queryDto.Type);

            query = query.OrderByDescending(t => t.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query.Skip((queryDto.Page - 1) * queryDto.PageSize).Take(queryDto.PageSize)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Type = t.Type.ToString(),
                    Reference = t.Reference,
                    CreatedAt = t.CreatedAt,
                    IsCredit = walletId.HasValue && t.ToWalletId == walletId
                })
                .ToListAsync();

            return new PagedResult<TransactionDto>
            {
                Page = queryDto.Page,
                PageSize = queryDto.PageSize,
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
