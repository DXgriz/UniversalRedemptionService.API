namespace UniversalRedemptionService.API.DTOs
{
    public class TransferRequestDto
    {
        public string RecipientEmail { get; set; } = null!;
        public decimal Amount { get; set; }

        // Idempotency key
        public string Reference { get; set; } = null!;
    }
}
