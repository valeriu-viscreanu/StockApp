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

        // Nullable: existing users have no role assigned
        public Guid? RoleID { get; set; }
        public virtual UserRole? Role { get; set; }

        public virtual Account? Account { get; set; }

        public virtual ICollection<BuyOrder> BuyOrders { get; set; } = new List<BuyOrder>();
        public virtual ICollection<SellOrder> SellOrders { get; set; } = new List<SellOrder>();
        public virtual ICollection<UserOperation> UserOperations { get; set; } = new List<UserOperation>();
    }
}
