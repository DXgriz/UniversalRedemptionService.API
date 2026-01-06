using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using UniversalRedemptionService.API.Data;
using UniversalRedemptionService.API.DTOs;
using UniversalRedemptionService.API.Services;

namespace UniversalRedemptionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController(TransactionService transactionService, SLUCRSDbContext context, TransactionExportService exportService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await transactionService.GetUserTransactionsAsync(userId, page, pageSize);

            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> MyTransactions([FromQuery] TransactionQueryDto query)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var walletId = await context.Wallets.Where(w => w.UserId == userId).Select(w => w.Id).FirstAsync();

            var result = await transactionService.QueryTransactionsAsync(walletId, query);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> AllTransactions([FromQuery] TransactionQueryDto query)
        {
            var result = await transactionService.QueryTransactionsAsync(null, query);

            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportCsv([FromQuery] TransactionQueryDto query)
        {
            var file = await exportService.ExportCsvAsync(null, query);
            return File(file, "text/csv", "transactions.csv");
        }
    }
}
