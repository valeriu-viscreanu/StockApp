using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.DTO;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class SpreadSettingsApiController : ControllerBase
    {
        private readonly ISpreadSettingRepository _spreadSettingRepository;

        public SpreadSettingsApiController(ISpreadSettingRepository spreadSettingRepository)
        {
            _spreadSettingRepository = spreadSettingRepository;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSpread([FromBody] UpdateSpreadRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "A spread percentage is required." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var saved = await _spreadSettingRepository.UpdateSpreadPercentageAsync(request.SpreadPercentage);

            return Ok(new
            {
                spreadPercentage = saved.SpreadPercentage,
                updatedAt = saved.UpdatedAt
            });
        }
    }
}
