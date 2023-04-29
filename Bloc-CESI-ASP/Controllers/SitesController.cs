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
    public class SitesController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SitesController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sites = await _applicationDbContext.Sites.ToListAsync();
            return View(sites);
        }  
        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSiteViewModel addSiteRequest)
        {
            var site = new Site()
            {
                Id = Guid.NewGuid(),
                City = addSiteRequest.City,
            };

            await _applicationDbContext.Sites.AddAsync(site);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
           var site = await _applicationDbContext.Sites.FirstOrDefaultAsync(x => x.Id == id);

           if (site != null)
           {
               var viewModel = new UpdateSiteViewModel()
               {
                   Id = site.Id,
                   City = site.City,
               };
               return await Task.Run(() => View("View", viewModel));
           }

           return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> View(UpdateSiteViewModel model)
        {
            var site = await _applicationDbContext.Sites.FindAsync(model.Id);

            if (site != null)
            {
                site.City = model.City;

                await _applicationDbContext.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateSiteViewModel model)
        {
            var site = await _applicationDbContext.Sites.FindAsync(model.Id);

            if (site != null)
            {
                _applicationDbContext.Sites.Remove(site);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}