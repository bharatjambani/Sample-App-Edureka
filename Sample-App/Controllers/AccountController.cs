using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sample_App.Models;
using Sample_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.Controllers
{
    //[Route("authorize")]
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, IMapper mapper, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        //[Route("login/{userName:minlength(4)}/{password:minlength(4)}")]

        [Route("login")]
        //public IActionResult Login(string userName, string password)
        public IActionResult Login()
        {
            return View();
            //return Content("Login Page");
        }
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            //return View();
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
            //return Content("Logout Page");
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
            //return Content("Register Page");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<ApplicationUser>(register);
                var result = await _userManager.CreateAsync(user, register.Password);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.TryAddModelError(item.Code, item.Description);
                    }
                    return View(register);
                }

                //await _userManager.AddToRoleAsync(user, "User");
                await _userManager.AddToRoleAsync(user, "Administrator");
                return RedirectToAction("Index", "Home");
            }
            return View();
            //return Content("Register Page");
        }
    }
}
