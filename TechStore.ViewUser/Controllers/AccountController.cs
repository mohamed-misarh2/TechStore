using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;
using TechStore.Dtos.AccountDtos;
using TechStore.Dtos.UserDTO;

namespace TechStore.ViewUser.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserServices _userServices;

        public AccountController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDto register, string RoleName="User")
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            var result= await _userServices.RegisterUser(register , RoleName);
            if (result.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View( "Register", register);

            }

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return View("Login",login);
            }
            
            var result = await _userServices.LoginUser(login);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index" ,"Home");
            }
            else
            {
                return View("Login",login);

            }

        }
        [Authorize]

        [HttpGet]
        public async Task<IActionResult> UpdateAccount(string Id )
        {
            var user =await _userServices.GetUserById(Id);
            return View(user) ;
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateAccountAsync(string Id, UpdateUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("UpdateAccount" , model);
            }
            var data = await _userServices.UpdateUser(Id,model);   
            return View(model);
        }
        public async Task<IActionResult> LogoutAsync()
        {
            await _userServices.LogoutUser();
            return RedirectToAction("Index", "Home");

        }

    }
}
