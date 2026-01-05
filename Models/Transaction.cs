namespace UniversalRedemptionService.API.Models
{

    public enum TransactionType
    {
        Redemption = 1,
        Transfer = 2,
        Payment = 3
    }
    public class Transaction
    {
        public int Id { get; set; }

        public int? FromWalletId { get; set; }
        public int? ToWalletId { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public string Reference { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
