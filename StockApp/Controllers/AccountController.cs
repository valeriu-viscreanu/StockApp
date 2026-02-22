using Microsoft.AspNetCore.Mvc;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;

namespace StockApp.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(loginRequest);
            }

            bool isSuccess = _accountService.Login(loginRequest);
            if (isSuccess)
            {
                // Note: Auth session setup is skipped per user request.
                // Normally we would call HttpContext.SignInAsync(...) here.
                return RedirectToAction("Index", "Trade");
            }

            ViewBag.Errors = new List<string> { "Invalid email or password" };
            return View(loginRequest);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Logout()
        {
            // Note: Auth session clear is skipped per user request.
            return RedirectToAction("Index", "Trade");
        }
    }
}
