using ASPNetMVC.Add;
using ASPNetMVC.Models;
using ASPNetMVC.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNetMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext _mVCDemoDbContext;

        public EmployeesController(MVCDemoDbContext mVCDemoDbContext)
        {
            _mVCDemoDbContext = mVCDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _mVCDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeViewModel)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeViewModel.Name,
                Email = addEmployeeViewModel.Email,
                Department = addEmployeeViewModel.Department,
                Salary = addEmployeeViewModel.Salary,
                DateOfBirth = addEmployeeViewModel.DateOfBirth
            };

            await _mVCDemoDbContext.Employees.AddAsync(employee);
            await _mVCDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid Id)
        {
            var employee = await _mVCDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == Id);
            
            if (employee != null)
            {
                var viewModel = new UpdateViewModel
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = employee.Department,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth
                };

                return await Task.Run(() => View("View", viewModel));
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateViewModel updateViewModel)
        {
            var employee = await _mVCDemoDbContext.Employees.FindAsync(updateViewModel.Id);

            if (employee != null)
            {
                employee.Email = updateViewModel.Email;
                employee.Salary = updateViewModel.Salary;
                employee.DateOfBirth = updateViewModel.DateOfBirth;
                employee.Name = updateViewModel.Name;
                employee.Department = updateViewModel.Department;

                await _mVCDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(UpdateViewModel updateViewModel)
        {
            var employee = await _mVCDemoDbContext.Employees.FindAsync(updateViewModel.Id);

            if (employee != null)
            {
                _mVCDemoDbContext.Employees.Remove(employee);
                await _mVCDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
