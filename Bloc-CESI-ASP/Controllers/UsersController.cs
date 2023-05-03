using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bloc_CESI_ASP.Data;
using Bloc_CESI_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloc_CESI_ASP.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UsersController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var viewModel = new AddUserViewModel();
            return View("~/Views/Register.cshtml",viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(AddUserViewModel addUserRequest)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = addUserRequest.Email,
                Password = addUserRequest.Password = addUserRequest.ConfirmedPassword != addUserRequest.Password ? "": BCrypt.Net.BCrypt.HashPassword(addUserRequest.Password)
            };
            
            var cookieOptions = new CookieOptions(); 
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            cookieOptions.Path = "/"; 
            Response.Cookies.Append("userToken", Guid.NewGuid().ToString(), cookieOptions);
            
            await _applicationDbContext.Users.AddAsync(user);
            await _applicationDbContext.SaveChangesAsync();

            return View("~/Views/Home/Index.cshtml");
        }  
        
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var viewModel = new LoginUserViewModel();
            return View("~/Views/Login.cshtml",viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(user => user.Email == model.Email);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                var cookieOptions = new CookieOptions(); 
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                cookieOptions.Path = "/"; 
                Response.Cookies.Append("userToken", Guid.NewGuid().ToString(), cookieOptions);
            };

            return View("~/Views/Home/Index.cshtml");
        }
        
        
    }
}