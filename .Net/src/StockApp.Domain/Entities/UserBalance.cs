namespace StockApp.Domain.Entities
{
    /// <summary>
    /// Represents the cash balance for a user, stored in a separate table from the user profile.
    /// </summary>
    public class UserBalance
    {
        public Guid UserID { get; set; }

        public double Balance { get; set; } = 1000.00;
    }
}
