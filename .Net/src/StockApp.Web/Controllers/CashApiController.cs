using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.ServiceContracts;
using System.Security.Claims;

namespace StockApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CashApiController : ControllerBase
    {
        private readonly IUserBalanceService _userBalanceService;

        public CashApiController(IUserBalanceService userBalanceService)
        {
            _userBalanceService = userBalanceService;
        }

        [HttpGet("balance")]
        public ActionResult<double> GetBalance()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                return Ok(_userBalanceService.GetBalance(userId));
            }
            return Unauthorized(new { message = "User ID not found in token." });
        }

        [HttpPost("add-funds")]
        public IActionResult AddFunds([FromBody] double amount)
        {
            if (amount <= 0)
            {
                return BadRequest(new { message = "Amount must be positive." });
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                _userBalanceService.AddBalance(userId, amount);
                return Ok(new { balance = _userBalanceService.GetBalance(userId) });
            }
            return Unauthorized(new { message = "User ID not found in token." });
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] double amount)
        {
            if (amount <= 0)
            {
                return BadRequest(new { message = "Amount must be positive." });
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                if (_userBalanceService.DeductBalance(userId, amount))
                {
                    return Ok(new { balance = _userBalanceService.GetBalance(userId) });
                }
                return BadRequest(new { message = "Insufficient funds." });
            }
            return Unauthorized(new { message = "User ID not found in token." });
        }
    }
}
