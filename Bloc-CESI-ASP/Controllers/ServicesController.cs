using Bloc_CESI_ASP.Data;
using Bloc_CESI_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloc_CESI_ASP.Controllers
{
    [Route("admin/services/[action]")]
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
            return View("~/Views/Admin/Services/Index.cshtml", sites);
        }  
        
        [HttpGet]
        public IActionResult Add()
        {
            return View("~/Views/Admin/Services/Add.cshtml");
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
               return await Task.Run(() => View("~/Views/Admin/Services/View.cshtml", viewModel));
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
            var employeeWithService = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.Service == model.Name);

            if (service != null && employeeWithService == null)
            {
                _applicationDbContext.Services.Remove(service);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}