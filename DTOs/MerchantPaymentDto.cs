namespace UniversalRedemptionService.API.DTOs
{
    public class MerchantPaymentDto
    {
        public string MerchantEmail { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Reference { get; set; } = Guid.NewGuid().ToString();
    }
}
