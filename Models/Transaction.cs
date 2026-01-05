using Microsoft.EntityFrameworkCore;

namespace UniversalRedemptionService.API.Models
{

    public enum TransactionType
    {
        Redemption = 1,
        Transfer = 2,
        Payment = 3,
        Withdrawal = 4
    }

    public enum TransactionStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3
    }


    [Index(nameof(Reference), IsUnique = true)]
    public class Transaction
    {
        public int Id { get; set; }

        public int? FromWalletId { get; set; }
        public Wallet? FromWallet { get; set; }

        public int? ToWalletId { get; set; }
        public Wallet? ToWallet { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public string Reference { get; set; } = null!;

        public TransactionStatus Status { get; set; } = TransactionStatus.Completed;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
