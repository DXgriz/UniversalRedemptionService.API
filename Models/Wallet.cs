namespace UniversalRedemptionService.API.Models
{
    public class Wallet
    {
        public int Id { get; set; }

        public decimal Balance { get; set; } = 0;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
