using UniversalRedemptionService.API.ProvidersModels;

namespace UniversalRedemptionService.API.Providers
{
    public interface ICashSendProvider
    {
        Task<CashSendValidationResult> ValidateAsync(
        string referenceNumber,
        string pin
    );
    }
}
