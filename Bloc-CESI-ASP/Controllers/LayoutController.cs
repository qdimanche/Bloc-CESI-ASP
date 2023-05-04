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
    public class LayoutController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LayoutController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var viewModel = new SearchViewModel();
            return View("~/Views/Shared/_Layout.cshtml",viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel searchRequest)
        {
            var request = searchRequest.Request?.ToLower();
            var services = await _applicationDbContext.Services.ToListAsync();
            var sites = await _applicationDbContext.Sites.ToListAsync();
            var employees = await _applicationDbContext.Employees.ToListAsync();
            var viewModel = new SearchViewModel();
            viewModel.Request = request;
            if (request != null)
            {
                foreach (var service in services)
                {
                    if (service.Name?.ToLower() == request)
                    {
                        return View("~/Views/Admin/Employees/View.cshtml/"+ service.Id);
                    }
                }   
                foreach (var employee in employees)
                {
                    if (employee.FirstName?.ToLower() == request || employee.FirstName?.ToLower()  == request || employee.Email?.ToLower()  == request)
                    {
                        return View("~/Views/Admin/Employees/View.cshtml/"+ employee.Id);
                    }
                }      
                foreach (var site in sites)
                {
                    if (site.City?.ToLower() == request)
                    {
                        return View("~/Views/Admin/Sites/View.cshtml/"+ site.Id);
                    }
                }
            }
            return View("~/Views/Home/Index.cshtml/", searchRequest);

        }
    }
}