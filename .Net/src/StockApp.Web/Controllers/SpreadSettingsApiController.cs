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
        /// <summary>
        /// A spread wider than this is almost certainly a mistake rather than a
        /// deliberate setting, so it is refused.
        /// </summary>
        private const double MaximumSpreadPercentage = 100.0;

        private readonly ISpreadSettingRepository _spreadSettingRepository;

        public SpreadSettingsApiController(ISpreadSettingRepository spreadSettingRepository)
        {
            _spreadSettingRepository = spreadSettingRepository;
        }

        [HttpPut]
        public IActionResult UpdateSpread([FromBody] UpdateSpreadRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "A spread percentage is required." });
            }

            if (double.IsNaN(request.SpreadPercentage) || double.IsInfinity(request.SpreadPercentage))
            {
                return BadRequest(new { message = "Spread percentage must be a number." });
            }

            if (request.SpreadPercentage < 0)
            {
                return BadRequest(new { message = "Spread percentage cannot be negative." });
            }

            if (request.SpreadPercentage > MaximumSpreadPercentage)
            {
                return BadRequest(new
                {
                    message = $"Spread percentage cannot exceed {MaximumSpreadPercentage}."
                });
            }

            var saved = _spreadSettingRepository.UpdateSpreadPercentage(request.SpreadPercentage);

            return Ok(new
            {
                spreadPercentage = saved.SpreadPercentage,
                updatedAt = saved.UpdatedAt
            });
        }
    }
}
