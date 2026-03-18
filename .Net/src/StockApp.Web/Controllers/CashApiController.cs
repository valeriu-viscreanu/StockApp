using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.ServiceContracts;
using System.Security.Claims;

namespace StockApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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
            // For testing: Fallback to fixed GUID if token parsing fails
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            }
            return Ok(_userBalanceService.GetBalance(userId));
        }

        [HttpPost("add-funds")]
        public IActionResult AddFunds([FromBody] double amount)
        {
            if (amount <= 0)
            {
                return BadRequest(new { message = "Amount must be positive." });
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            }
            
            _userBalanceService.AddBalance(userId, amount);
            return Ok(new { balance = _userBalanceService.GetBalance(userId) });
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] double amount)
        {
            if (amount <= 0)
            {
                return BadRequest(new { message = "Amount must be positive." });
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            }

            if (_userBalanceService.DeductBalance(userId, amount))
            {
                return Ok(new { balance = _userBalanceService.GetBalance(userId) });
            }
            return BadRequest(new { message = "Insufficient funds." });
        }
    }
}
