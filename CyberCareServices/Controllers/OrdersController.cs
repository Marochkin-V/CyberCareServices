using CyberCareServices.Data;
using CyberCareServices.Model;
using CyberCareServices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CyberCareServices.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CyberCareServicesContext _context;
        private readonly int PageSize = 20;

        public OrdersController(CyberCareServicesContext context)
        {
            _context = context;
        }

        // GET: OrdersController
        [ResponseCache(Duration = 2 * 5 + 240, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var orders = await _context.Orders
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Select(o => new OrderViewModel
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    CompletionDate = o.CompletionDate,
                    CustomerName = o.Customer.FullName,
                    Prepayment = o.Prepayment,
                    PaymentStatus = o.PaymentStatus,
                    CompletionStatus = o.CompletionStatus,
                    TotalCost = o.TotalCost,
                    WarrantyPeriod = o.WarrantyPeriod,
                    EmployeeName = o.Employee.FullName,
                })
                .ToListAsync();

            var ordersvm = new OrdersViewModel
            {
                Orders = orders,
                PageViewModel = new PageViewModel(_context.Orders.Count(), page, PageSize)
            };

            return View(ordersvm);
        }

        // GET: OrdersController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new OrderViewModel
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                CompletionDate = order.CompletionDate,
                CustomerName = order.Customer.FullName,
                Prepayment = order.Prepayment,
                PaymentStatus = order.PaymentStatus,
                CompletionStatus = order.CompletionStatus,
                TotalCost = order.TotalCost,
                WarrantyPeriod = order.WarrantyPeriod,
                EmployeeName = order.Employee.FullName,
            };

            return View(viewModel);
        }

        // GET: OrdersController/Create
        public IActionResult Create()
        {
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Employees"] = _context.Employees.ToList();
            return View();
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Employees"] = _context.Employees.ToList();
            return View(order);
        }

        // GET: OrdersController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Employees"] = _context.Employees.ToList();
            return View(order);
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Orders.Any(e => e.OrderId == order.OrderId))
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

            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Employees"] = _context.Employees.ToList();
            return View(order);
        }

        // GET: OrdersController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: OrdersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
