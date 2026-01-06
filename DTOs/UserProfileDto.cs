namespace UniversalRedemptionService.API.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public decimal Balance { get; set; }
        public bool IsMerchant { get; set; }
    }
}
