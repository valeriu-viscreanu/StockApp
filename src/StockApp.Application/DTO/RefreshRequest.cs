using System.ComponentModel.DataAnnotations;

namespace StockApp.Application.DTO
{
    public class RefreshRequest
    {
        [Required(ErrorMessage = "Refresh token is required.")]
        public string? RefreshToken { get; set; }
    }
}
