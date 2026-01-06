using System.ComponentModel.DataAnnotations;

namespace UniversalRedemptionService.API.DTOs
{
    public class RedeemCashSendDto
    {
        [Required]
        public string Provider { get; set; } = null!;
        [Required]
        public string ReferenceNumber { get; set; } = null!;
        [Required]
        public string Pin { get; set; } = null!;
    }
}
