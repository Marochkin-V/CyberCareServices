using Microsoft.AspNetCore.Mvc;
using CyberCareServices.Model;
using CyberCareServices.ViewModels;
using Microsoft.EntityFrameworkCore;
using CyberCareServices.Data;
using Microsoft.AspNetCore.Authorization;

namespace CyberCareServices.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly CyberCareServicesContext _context;
        private readonly int PageSize = 20;

        public CustomersController(CyberCareServicesContext context)
        {
            _context = context;
        }

        // GET: Customer
        [ResponseCache(Duration = 2 * 5 + 240, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index(string searchQuery, int page = 1)
        {
            // Базовый запрос для выборки клиентов
            var customersQuery = _context.Customers.AsQueryable();

            // Фильтрация по имени, если задан поисковый запрос
            if (!string.IsNullOrEmpty(searchQuery))
            {
                customersQuery = customersQuery.Where(c => c.FullName.Contains(searchQuery));
            }

            // Пагинация
            var customers = await customersQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var viewModel = new CustomersViewModel
            {
                Customers = customers,
                PageViewModel = new PageViewModel(customersQuery.Count(), page, PageSize),
                SearchQuery = searchQuery // Добавляем поисковый запрос в модель
            };

            return View(viewModel);
        }


        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Address,Phone,DiscountAvailable,DiscountAmount")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FullName,Address,Phone,DiscountAvailable,DiscountAmount")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
