namespace UniversalRedemptionService.API.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
        public string Reference { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsCredit { get; set; }
    }
}
