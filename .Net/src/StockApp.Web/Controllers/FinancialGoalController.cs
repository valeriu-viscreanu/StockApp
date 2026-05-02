using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using System.Security.Claims;

namespace StockApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FinancialGoalController : ControllerBase
    {
        private readonly IFinancialGoalService _financialGoalService;

        public FinancialGoalController(IFinancialGoalService financialGoalService)
        {
            _financialGoalService = financialGoalService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FinancialGoalResponse>>> GetGoals()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                var goals = await _financialGoalService.GetGoalsByUserId(userId);
                return Ok(goals);
            }
            return Unauthorized(new { message = "User ID not found in token." });
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<GoalTypeResponse>>> GetGoalTypes()
        {
            var types = await _financialGoalService.GetGoalTypes();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialGoalResponse>> GetGoal(Guid id)
        {
            var goal = await _financialGoalService.GetGoalById(id);
            if (goal == null) return NotFound();
            return Ok(goal);
        }

        [HttpPost]
        public async Task<ActionResult<FinancialGoalResponse>> CreateGoal([FromBody] FinancialGoalRequest request)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                var goal = await _financialGoalService.CreateGoal(request, userId);
                return CreatedAtAction(nameof(GetGoal), new { id = goal.FinancialGoalID }, goal);
            }
            return Unauthorized(new { message = "User ID not found in token." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoal(Guid id, [FromBody] FinancialGoalRequest request)
        {
            var success = await _financialGoalService.UpdateGoal(id, request);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(Guid id)
        {
            var success = await _financialGoalService.DeleteGoal(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/contribution")]
        public async Task<IActionResult> AddContribution(Guid id, [FromBody] double amount)
        {
            if (amount <= 0) return BadRequest(new { message = "Amount must be positive." });
            
            var success = await _financialGoalService.AddContribution(id, amount);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
