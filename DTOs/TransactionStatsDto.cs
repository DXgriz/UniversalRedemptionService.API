namespace UniversalRedemptionService.API.DTOs
{
    public class TransactionStatsDto
    {
        public int TotalTransactions { get; set; }
        public decimal TotalAmount { get; set; }

        public Dictionary<string, int> CountByType { get; set; } = new();
    }
}
