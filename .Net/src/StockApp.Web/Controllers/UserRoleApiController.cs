using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Infrastructure.Database;

namespace StockApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserRoleApiController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRoleApiController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _dbContext.UserRoles
                .Select(r => new { r.RoleID, r.RoleName })
                .ToList();
            return Ok(roles);
        }
    }
}
