using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.Repositories;
using MyApp.ViewModels;

namespace MyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Address = model.Address,
                    MobileNumber = model.MobileNumber,
                    ZodiacSign = model.ZodiacSign
                };

                var result = await _userRepository.RegisterUserAsync(user, model.Password);
                return RedirectToAction("Login", "Account");

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.FindUserByEmailAsync(model.Email);
                if (user != null) {

                var result = await _userRepository.SignInUserAsync(user.UserName, model.Password, model.RememberMe);

                if (result.Succeeded)
                {
                    return RedirectToAction("GetBooks", "Books");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "User account locked out.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }
        }

    return View(model);
    }

    [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userRepository.SignOutUserAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
