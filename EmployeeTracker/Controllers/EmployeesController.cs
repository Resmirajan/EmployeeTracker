using EmployeeTracker.Data;
using EmployeeTracker.Models;
using EmployeeTracker.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Controllers
{
    public class EmployeesController : Controller
    {
        //dependancy injection
        private readonly MVCDemoDBContect mvcDemoDBContect;

        public EmployeesController(MVCDemoDBContect mvcDemoDBContect)
        {
            this.mvcDemoDBContect = mvcDemoDBContect;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        //display list of employees
        [HttpGet]
         public async Task<IActionResult> Index()
        {
          var employees= await mvcDemoDBContect.Employees.ToListAsync();
            return View(employees);
        }
        //view a single employee detail
        [HttpGet]
        public async Task <IActionResult> View(Guid id)
        {
           var employee= await mvcDemoDBContect.Employees.FirstOrDefaultAsync(u=>u.Id==id);
            if (employee!=null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id= employee.Id,
                    Name=employee.Name,
                    Email=employee.Email,
                    Salary=employee.Salary,
                    DateOfBirth=employee.DateOfBirth,
                    Department=employee.Department

                };
                return await Task.Run(()=> View("View",viewModel));
            }
            return RedirectToAction("Index");
          
        }
        //add employee details
        [HttpPost]
        public async Task< IActionResult >Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department
            };
           await mvcDemoDBContect.Employees.AddAsync(employee);
           await mvcDemoDBContect.SaveChangesAsync();
            return RedirectToAction("Add");
        }
       //update details
        [HttpPost]
        public async Task<ActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDBContect.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;
                await mvcDemoDBContect.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult>Delete(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDBContect.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                mvcDemoDBContect.Employees.Remove(employee);
                await mvcDemoDBContect.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
    
}
