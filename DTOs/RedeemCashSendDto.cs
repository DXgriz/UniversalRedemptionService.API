namespace UniversalRedemptionService.API.DTOs
{
    public class RedeemCashSendDto
    {
        public string Provider { get; set; } = null!;
        public string ReferenceNumber { get; set; } = null!;

        public string Pin { get; set; } = null!;
    }
}
