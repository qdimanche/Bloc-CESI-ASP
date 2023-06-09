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
    [Route("admin/sites/[action]")]
    public class SitesController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SitesController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(SiteViewModel indexSiteRequest)
        {
            var sites = await _applicationDbContext.Sites.ToListAsync();
            indexSiteRequest.Sites = sites;

            return View("~/Views/admin/sites/index.cshtml",indexSiteRequest);
        }  
        
        [HttpGet]
        public IActionResult Add()
        {
            return View("~/Views/admin/sites/add.cshtml");
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
               return await Task.Run(() => View("~/views/admin/sites/view.cshtml", viewModel));
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
            var employeeWithSite = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.Site == model.City);


            if (site != null && employeeWithSite == null)
            {
                _applicationDbContext.Sites.Remove(site);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Search(SiteViewModel searchSiteRequest)
        {
            var sites = await _applicationDbContext.Sites.Where( x=>
                x.City == searchSiteRequest.Request).ToListAsync();

            searchSiteRequest.Sites = sites;
            
            return View("~/Views/admin/sites/index.cshtml", searchSiteRequest);
        }
    }
}