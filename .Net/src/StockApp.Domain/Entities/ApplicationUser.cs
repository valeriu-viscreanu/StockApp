using System;
using System.Collections.Generic;

namespace StockApp.Domain.Entities
{
    /// <summary>
    /// Represents a user in the application who can own buy and sell orders.
    /// </summary>
    public class ApplicationUser
    {
        public Guid UserID { get; set; }

        public string Email { get; set; } = string.Empty;

        public virtual ICollection<BuyOrder> BuyOrders { get; set; } = new List<BuyOrder>();
        public virtual ICollection<SellOrder> SellOrders { get; set; } = new List<SellOrder>();
    }
}
