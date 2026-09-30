using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Application.Services;
using StockApp.Controllers;
using StockAppTests.Mocks;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace StockAppTests
{
    /// <summary>
    /// The spread is a money-affecting setting, so the endpoint that changes it is
    /// restricted to admins and refuses values that would make pricing nonsensical.
    /// </summary>
    public class SpreadSettingsApiControllerTests
    {
        private readonly InMemorySpreadSettingRepository _repository = new(spreadPercentage: 0.2);
        private readonly SpreadSettingsApiController _controller;

        public SpreadSettingsApiControllerTests()
        {
            _controller = new SpreadSettingsApiController(_repository);
        }

        private static UpdateSpreadRequest Request(double spreadPercentage) =>
            new() { SpreadPercentage = spreadPercentage };

        // Replicates the [ApiController] model-binding validation step, which a
        // direct call to the action bypasses.
        private async Task<IActionResult> UpdateSpread(UpdateSpreadRequest? request)
        {
            _controller.ModelState.Clear();
            if (request != null)
            {
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(
                    request,
                    new ValidationContext(request),
                    validationResults,
                    validateAllProperties: true);

                foreach (var result in validationResults)
                {
                    foreach (var memberName in result.MemberNames.DefaultIfEmpty(string.Empty))
                    {
                        _controller.ModelState.AddModelError(memberName, result.ErrorMessage ?? "Invalid value.");
                    }
                }
            }

            return await _controller.UpdateSpread(request);
        }

        // 1. Only admins may change the spread.
        [Fact]
        public void Controller_RequiresTheAdminRole()
        {
            var authorize = typeof(SpreadSettingsApiController)
                .GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(authorize);
            Assert.Equal("Admin", authorize!.Roles);
        }

        // 2. A valid spread is persisted and echoed back.
        [Fact]
        public async Task UpdateSpread_ValidPercentage_PersistsTheNewValue()
        {
            IActionResult result = await UpdateSpread(Request(0.5));

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0.5, _repository.GetCurrent()!.SpreadPercentage, precision: 10);
        }

        // 3. A negative spread would invert bid and ask, so it is refused.
        [Fact]
        public async Task UpdateSpread_NegativePercentage_IsRejectedAndNothingChanges()
        {
            IActionResult result = await UpdateSpread(Request(-0.1));

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(0.2, _repository.GetCurrent()!.SpreadPercentage, precision: 10);
        }

        // 4. An implausibly wide spread is refused rather than silently applied.
        [Fact]
        public async Task UpdateSpread_AboveMaximum_IsRejectedAndNothingChanges()
        {
            IActionResult result = await UpdateSpread(Request(100.1));

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(0.2, _repository.GetCurrent()!.SpreadPercentage, precision: 10);
        }

        // 5. A missing body is refused rather than throwing.
        [Fact]
        public async Task UpdateSpread_NullRequest_IsRejected()
        {
            IActionResult result = await UpdateSpread(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // 6. Zero is a legitimate setting - it means trade at mid.
        [Fact]
        public async Task UpdateSpread_Zero_IsAccepted()
        {
            IActionResult result = await UpdateSpread(Request(0));

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(0, _repository.GetCurrent()!.SpreadPercentage, precision: 10);
        }

        // 7. The saved value is what trading actually prices against, with no restart.
        [Fact]
        public async Task UpdateSpread_TakesEffectOnTheNextPricedTrade()
        {
            ISpreadPricingService pricing = new SpreadPricingService(_repository);
            Assert.Equal(100.10, await pricing.GetAskPriceAsync(100.0), precision: 10);

            await UpdateSpread(Request(1.0));

            Assert.Equal(100.50, await pricing.GetAskPriceAsync(100.0), precision: 10);
        }
    }
}
