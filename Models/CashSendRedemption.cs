namespace UniversalRedemptionService.API.Models
{
    public class CashSendRedemption
    {
        public int Id { get; set; }

        public string Provider { get; set; } = null!;
        public string ReferenceNumber { get; set; } = null!;
        public string PinHash { get; set; } = null!;

        public decimal Amount { get; set; }

        public bool IsRedeemed { get; set; } = false;
        public DateTime? RedeemedAt { get; set; }

        public int? WalletId { get; set; }
        public Wallet? Wallet { get; set; } = null!;
    }
}
