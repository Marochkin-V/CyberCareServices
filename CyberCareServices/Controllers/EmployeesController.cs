using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CyberCareServices.Model;
using CyberCareServices.ViewModels;
using CyberCareServices.Data;
using Microsoft.AspNetCore.Authorization;
using System.Drawing.Printing;

namespace CyberCareServices.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly CyberCareServicesContext _context;

        public EmployeesController(CyberCareServicesContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string sortField, string sortOrder, string searchQuery)
        {
            // Устанавливаем значения по умолчанию для сортировки
            sortField = string.IsNullOrEmpty(sortField) ? "FullName" : sortField;
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "asc" : sortOrder;

            // Начальная выборка сотрудников
            var employeesQuery = _context.Employees
                .Select(e => new Employee
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Position = e.Position,
                    DateOfHire = e.DateOfHire,
                    DateOfBirth = e.DateOfBirth,
                    Phone = e.Phone,
                    Email = e.Email
                });

            // Фильтрация по поисковому запросу
            if (!string.IsNullOrEmpty(searchQuery))
            {
                employeesQuery = employeesQuery.Where(e => e.FullName.Contains(searchQuery));
            }

            // Сортировка по выбранному полю и направлению
            if (sortOrder == "desc")
            {
                employeesQuery = sortField switch
                {
                    "FullName" => employeesQuery.OrderByDescending(e => e.FullName),
                    "Position" => employeesQuery.OrderByDescending(e => e.Position),
                    _ => employeesQuery.OrderByDescending(e => e.FullName),
                };
            }
            else
            {
                employeesQuery = sortField switch
                {
                    "FullName" => employeesQuery.OrderBy(e => e.FullName),
                    "Position" => employeesQuery.OrderBy(e => e.Position),
                    _ => employeesQuery.OrderBy(e => e.FullName),
                };
            }

            var employees = await employeesQuery.ToListAsync();

            var employeevm = new EmployeesViewModel()
            {
                Employees = employees,
                SortField = sortField,
                SortOrder = sortOrder,
                SearchQuery = searchQuery
            };

            return View(employeevm);
        }



        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FullName,Position,DateOfHire,DateOfBirth,Phone,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FullName,Position,DateOfHire,DateOfBirth,Phone,Email")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
