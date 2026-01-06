namespace UniversalRedemptionService.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "User";
        public bool IsMerchant { get; set; } = false;



        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Wallet Wallet { get; set; } = null!;
    }
}
