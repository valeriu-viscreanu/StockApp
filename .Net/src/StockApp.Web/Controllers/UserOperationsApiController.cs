using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Domain.RepositoryContracts;
using System.Security.Claims;

namespace StockApp.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserOperationsApiController : ControllerBase
    {
        private readonly IUserOperationRepository _userOperationRepository;

        public UserOperationsApiController(IUserOperationRepository userOperationRepository)
        {
            _userOperationRepository = userOperationRepository;
        }

        [HttpGet]
        public IActionResult GetOperations()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                var operations = _userOperationRepository.GetByUserId(userId);
                return Ok(operations);
            }
            return Unauthorized(new { message = "User ID not found in token." });
        }
    }
}
