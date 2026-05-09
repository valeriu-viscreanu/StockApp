using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.ServiceContracts;
using System.Security.Claims;

namespace StockApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AdvisorApiController : ControllerBase
    {
        private readonly IAdvisorService _advisorService;

        public AdvisorApiController(IAdvisorService advisorService)
        {
            _advisorService = advisorService;
        }

        private Guid GetUserID()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdClaim, out var id) ? id : Guid.Empty;
        }

        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
        {
            var advisorId = GetUserID();
            if (advisorId == Guid.Empty) return Unauthorized();

            var clients = await _advisorService.GetClientsForAdvisorAsync(advisorId);
            return Ok(clients);
        }

        [HttpPost("clients/{clientId}")]
        public async Task<IActionResult> AssignClient(Guid clientId)
        {
            var advisorId = GetUserID();
            if (advisorId == Guid.Empty) return Unauthorized();

            var success = await _advisorService.AssignClientAsync(advisorId, clientId);
            if (!success) return BadRequest(new { message = "Client not found or assignment failed" });

            return Ok(new { message = "Client assigned successfully" });
        }

        [HttpDelete("clients/{clientId}")]
        public async Task<IActionResult> UnassignClient(Guid clientId)
        {
            var advisorId = GetUserID();
            if (advisorId == Guid.Empty) return Unauthorized();

            var success = await _advisorService.UnassignClientAsync(advisorId, clientId);
            if (!success) return BadRequest(new { message = "Client not found or unassignment failed" });

            return NoContent();
        }
    }
}
