using Bloc_CESI_ASP.Data;
using Bloc_CESI_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloc_CESI_ASP.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ServicesController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sites = await _applicationDbContext.Services.ToListAsync();
            return View(sites);
        }  
        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddServiceViewModel addServiceRequest)
        {
            var service = new Service()
            {
                Id = Guid.NewGuid(),
                Name = addServiceRequest.Name,
            };

            await _applicationDbContext.Services.AddAsync(service);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
           var service = await _applicationDbContext.Services.FirstOrDefaultAsync(x => x.Id == id);

           if (service != null)
           {
               var viewModel = new UpdateServiceViewModel()
               {
                   Id = service.Id,
                   Name = service.Name,
               };
               return await Task.Run(() => View("View", viewModel));
           }

           return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> View(UpdateServiceViewModel model)
        {
            var service = await _applicationDbContext.Services.FindAsync(model.Id);

            if (service != null)
            {
                service.Name = model.Name;

                await _applicationDbContext.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateServiceViewModel model)
        {
            var service = await _applicationDbContext.Services.FindAsync(model.Id);

            if (service != null)
            {
                _applicationDbContext.Services.Remove(service);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}