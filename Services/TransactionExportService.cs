using System.Text;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Services;

namespace UniversalRedemptionService.API.Services
{
    public class TransactionExportService(TransactionService transactionService)
    {
        public async Task<byte[]> ExportCsvAsync(int? walletId, TransactionQueryDto query)
        {
            query.Page = 1;
            query.PageSize = int.MaxValue;

            var result = await transactionService.QueryTransactionsAsync(walletId, query);

            var csv = new StringBuilder();
            csv.AppendLine("Id,Amount,Type,Reference,Date,IsCredit");

            foreach (var t in result.Items)
            {
                csv.AppendLine($"{t.Id},{t.Amount},{t.Type},{t.Reference},{t.CreatedAt},{t.IsCredit}");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }
    }
}
