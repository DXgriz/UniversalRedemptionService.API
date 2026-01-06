using Microsoft.EntityFrameworkCore;
using UniversalRedemptionService.API.Data;
using UniversalRedemptionService.API.DTOs;

namespace UniversalRedemptionService.API.Services
{
    public class TransactionStatsService(SLUCRSDbContext context)
    {
        public async Task<TransactionStatsDto> GetStatsAsync()
        {
            var totalTransactions = await context.Transactions.CountAsync();
            var totalAmount = await context.Transactions.SumAsync(t => t.Amount);

            var byType = await context.Transactions.GroupBy(t => t.Type)
                .Select(g => new
                {
                    Type = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.Type, x => x.Count);

            return new TransactionStatsDto
            {
                TotalTransactions = totalTransactions,
                TotalAmount = totalAmount,
                CountByType = byType
            };
        }
    }
}
