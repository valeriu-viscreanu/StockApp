using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
{
    public class Account
    {
        [Key]
        public Guid AccountID { get; set; }

        public Guid UserID { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        public double Balance { get; set; }

        public virtual ICollection<Cash> CashAllocations { get; set; } = new List<Cash>();
    }
}
