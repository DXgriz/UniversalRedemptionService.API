using UniversalRedemptionService.API.ProvidersModels;

namespace UniversalRedemptionService.API.Providers
{
    public class MockCashSendProvider : ICashSendProvider
    {
        public Task<CashSendValidationResult> ValidateAsync(
        string referenceNumber,
        string pin)
        {
            // Fake rules for demo
            if (referenceNumber.StartsWith("CS") && pin == "1234")
            {
                return Task.FromResult(new CashSendValidationResult
                {
                    IsValid = true,
                    Amount = 500,
                    ProviderName = "MockBank"
                });
            }

            return Task.FromResult(new CashSendValidationResult
            {
                IsValid = false
            });
        }
    }
}
