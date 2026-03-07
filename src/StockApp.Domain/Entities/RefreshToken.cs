namespace StockApp.Domain.Entities
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpiresUtc { get; set; }
        public bool IsRevoked { get; set; }
    }
}
