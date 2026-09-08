using System;
using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
{
    /// <summary>
    /// The trading spread, held in the database so it can be changed without a
    /// redeploy. A single row is expected; the most recently updated one wins.
    /// </summary>
    public class SpreadSetting
    {
        [Key]
        public Guid SpreadSettingID { get; set; }

        /// <summary>
        /// Full bid-ask spread as a percentage of the mid price (0.20 means 0.20%).
        /// Half is applied either side of mid.
        /// </summary>
        public double SpreadPercentage { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
