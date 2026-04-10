using System.ComponentModel.DataAnnotations;

namespace StockApp.Application.DTO
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Name can't be blank")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        // User Details
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

        // Optional: if not provided, user is created with no role
        public Guid? RoleID { get; set; }
    }
}
