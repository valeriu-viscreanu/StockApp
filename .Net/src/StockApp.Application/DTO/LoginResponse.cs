using System;

namespace StockApp.Application.DTO
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public Guid UserID { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }
    }
}
