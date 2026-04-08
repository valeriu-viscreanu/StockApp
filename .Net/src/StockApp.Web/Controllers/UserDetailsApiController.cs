using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.DTO;
using StockApp.Domain.RepositoryContracts;
using System.Security.Claims;

namespace StockApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class UserDetailsApiController : ControllerBase
    {
        private readonly IUserDetailsRepository _userDetailsRepository;

        public UserDetailsApiController(IUserDetailsRepository userDetailsRepository)
        {
            _userDetailsRepository = userDetailsRepository;
        }

        [HttpGet]
        public ActionResult<UserDetailsResponse> GetUserDetails()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized();
            }

            var details = _userDetailsRepository.GetByUserID(userId);
            if (details == null)
            {
                return Ok(new UserDetailsResponse());
            }

            return Ok(new UserDetailsResponse
            {
                Street = details.Street,
                StreetNumber = details.StreetNumber,
                Building = details.Building,
                Unit = details.Unit,
                City = details.City,
                ZipCode = details.ZipCode,
                Country = details.Country,
                AdditionalInfo = details.AdditionalInfo,
                PhoneNumber = details.PhoneNumber
            });
        }

        [HttpPost]
        public ActionResult UpdateUserDetails(UserDetailsResponse updateRequest)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized();
            }

            var details = _userDetailsRepository.GetByUserID(userId);
            if (details == null)
            {
                _userDetailsRepository.Add(new Domain.Entities.UserDetails
                {
                    DetailsID = Guid.NewGuid(),
                    UserID = userId,
                    Street = updateRequest.Street,
                    StreetNumber = updateRequest.StreetNumber,
                    Building = updateRequest.Building,
                    Unit = updateRequest.Unit,
                    City = updateRequest.City,
                    ZipCode = updateRequest.ZipCode,
                    Country = updateRequest.Country,
                    AdditionalInfo = updateRequest.AdditionalInfo,
                    PhoneNumber = updateRequest.PhoneNumber
                });
            }
            else
            {
                details.Street = updateRequest.Street;
                details.StreetNumber = updateRequest.StreetNumber;
                details.Building = updateRequest.Building;
                details.Unit = updateRequest.Unit;
                details.City = updateRequest.City;
                details.ZipCode = updateRequest.ZipCode;
                details.Country = updateRequest.Country;
                details.AdditionalInfo = updateRequest.AdditionalInfo;
                details.PhoneNumber = updateRequest.PhoneNumber;
                _userDetailsRepository.Update(details);
            }

            return Ok();
        }
    }
}
