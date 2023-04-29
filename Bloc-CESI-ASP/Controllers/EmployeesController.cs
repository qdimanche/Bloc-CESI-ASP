using Bloc_CESI_ASP.Data;
using Bloc_CESI_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloc_CESI_ASP.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EmployeesController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _applicationDbContext.Employees.ToListAsync();
            return View(employees);
        }  
        
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var services = await _applicationDbContext.Services.ToListAsync();
            var viewModel = new AddEmployeeViewModel()
            {
                Services = services
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                FirstName = addEmployeeRequest.FirstName,
                LastName = addEmployeeRequest.LastName,
                LandlinePhone = addEmployeeRequest.LandlinePhone,
                MobilePhone = addEmployeeRequest.MobilePhone,
                Email = addEmployeeRequest.Email,
                Service = addEmployeeRequest.SelectedService,
                Site = null,
            };

            await _applicationDbContext.Employees.AddAsync(employee);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
           var employee = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

           if (employee != null)
           {
               var viewModel = new UpdateEmployeeViewModel()
               {
                   Id = employee.Id,
                   FirstName = employee.FirstName,
                   LastName = employee.LastName,
                   LandlinePhone = employee.LandlinePhone,
                   MobilePhone = employee.MobilePhone,
                   Email = employee.Email,
                   Services = null,
                   Sites = null,
               };
               return await Task.Run(() => View("View", viewModel));
           }

           return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await _applicationDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.LandlinePhone = model.LandlinePhone;
                employee.MobilePhone = model.MobilePhone;
                employee.Email = model.Email;
                employee.Service = model.SelectedService;
                employee.Site = null;

                await _applicationDbContext.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await _applicationDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                _applicationDbContext.Employees.Remove(employee);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}