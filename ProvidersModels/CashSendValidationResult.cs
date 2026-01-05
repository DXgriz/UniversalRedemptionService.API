namespace UniversalRedemptionService.API.ProvidersModels
{
    public class CashSendValidationResult
    {
        public bool IsValid { get; set; }
        public decimal Amount { get; set; }
        public string ProviderName { get; set; } = null!;
    }
}
