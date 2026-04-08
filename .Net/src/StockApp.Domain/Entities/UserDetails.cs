using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
{
    public class UserDetails
    {
        [Key]
        public Guid DetailsID { get; set; }

        public Guid UserID { get; set; }

        [MaxLength(100)]
        public string? Street { get; set; }

        [MaxLength(20)]
        public string? StreetNumber { get; set; }

        [MaxLength(50)]
        public string? Building { get; set; }

        [MaxLength(20)]
        public string? Unit { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(20)]
        public string? ZipCode { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(500)]
        public string? AdditionalInfo { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
}
