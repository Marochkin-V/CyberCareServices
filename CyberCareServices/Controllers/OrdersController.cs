using CyberCareServices.Data;
using CyberCareServices.Model;
using CyberCareServices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

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
                .Include(o => o.Components)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            foreach (var component in order.Components)
            {
                component.ComponentType = _context.ComponentTypes.FirstOrDefault(ct => ct.ComponentTypeId == component.ComponentTypeId);
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
                Components = order.Components
                .Select(c => new ComponentViewModel
                {
                    ComponentId = c.ComponentId,
                    ComponentType = c.ComponentType.Name,
                    Brand = c.Brand,
                    Manufacturer = c.Manufacturer,
                    CountryOfOrigin = c.CountryOfOrigin,
                    ReleaseDate = c.ReleaseDate,
                    Specifications = c.Specifications,
                    WarrantyPeriod = c.WarrantyPeriod,
                    Description = c.Description,
                    Price = c.Price
                }).ToList(),
            };

            return View(viewModel);
        }

        public async Task<IActionResult> DeleteComponent(int orderid, int componentid)
        {
            var order = await _context.Orders
                .Include(o => o.Components)
                .FirstOrDefaultAsync(o => o.OrderId == orderid);

            if (order == null)
            {
                return NotFound();
            }

            var componentToRemove = order.Components.FirstOrDefault(c => c.ComponentId == componentid);

            if (componentToRemove == null)
            {
                return NotFound();
            }

            order.Components.Remove(componentToRemove);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = orderid });
        }

        // GET: OrdersController/AddComponent/5
        public async Task<IActionResult> AddComponent(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Components)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var availableComponents = await _context.Components.ToListAsync();

            var viewModel = new AddComponentViewModel
            {
                OrderId = orderId,
                AvailableComponents = new SelectList(availableComponents, "ComponentId", "Specifications")
            };

            return View(viewModel);
        }

        // POST: OrdersController/AddComponent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComponent([Bind("OrderId, ComponentId")] AddComponentViewModel model)
        {
            var order = await _context.Orders
                .Include(o => o.Components)
                .FirstOrDefaultAsync(o => o.OrderId == model.OrderId);

            if (order == null)
            {
                return NotFound();
            }

            var component = await _context.Components
                .FirstOrDefaultAsync(c => c.ComponentId == model.ComponentId);

            if (component == null)
            {
                ModelState.AddModelError("ComponentId", "Invalid component.");
                return View(model);
            }

            order.Components.Add(component);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = model.OrderId });
        }


        // GET: OrdersController/Create
        public async Task<IActionResult> Create()
        {
            var order = new OrderEditViewModel();

            if (order == null)
            {
                return NotFound();
            }

            order.Customers = await _context.Customers.ToListAsync();
            order.Employees = await _context.Employees.ToListAsync();


            return View(order);
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,CompletionDate,CustomerName,EmployeeName,Prepayment,PaymentStatus,CompletionStatus,TotalCost,WarrantyPeriod")] OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(ct => ct.FullName == model.CustomerName);

                if (customer == null)
                {
                    ModelState.AddModelError("Customer name", "Неверное имя клиента.");
                    return View(model);
                }

                var employee = await _context.Employees
                    .FirstOrDefaultAsync(ct => ct.FullName == model.EmployeeName);

                if (customer == null)
                {
                    ModelState.AddModelError("Employee name", "Неверное имя сотрудника.");
                    return View(model);
                }

                var order = new Order
                {
                    OrderId = model.OrderId,
                    OrderDate = model.OrderDate,
                    CompletionDate = model.CompletionDate,
                    CustomerId = customer.CustomerId,
                    EmployeeId = employee.EmployeeId,
                    Prepayment = model.Prepayment,
                    PaymentStatus = model.PaymentStatus,
                    CompletionStatus = model.CompletionStatus,
                    TotalCost = model.TotalCost,
                    WarrantyPeriod = model.WarrantyPeriod,
                };

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: OrdersController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Select(o => new OrderEditViewModel
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    CompletionDate = o.CompletionDate,
                    CustomerName = o.Customer.FullName,
                    EmployeeName = o.Employee.FullName,
                    Prepayment = o.Prepayment,
                    PaymentStatus = o.PaymentStatus,
                    CompletionStatus = o.CompletionStatus,
                    TotalCost = o.TotalCost,
                    WarrantyPeriod = o.WarrantyPeriod,
                })
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            order.Customers = await _context.Customers.ToListAsync();
            order.Employees = await _context.Employees.ToListAsync();


            return View(order);
        }


        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,CompletionDate,CustomerName,EmployeeName,Prepayment,PaymentStatus,CompletionStatus,TotalCost,WarrantyPeriod")] OrderViewModel model)
        {
            if (id != model.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var order = await _context.Orders
                        .Include(o => o.Customer)
                        .Include(o => o.Employee)
                        .FirstOrDefaultAsync(o => o.OrderId == id);

                    if (order == null)
                    {
                        return NotFound();
                    }

                    var customer = await _context.Customers
                        .FirstOrDefaultAsync(c => c.FullName == model.CustomerName);
                    if (customer == null)
                    {
                        ModelState.AddModelError("CustomerName", "Invalid customer name.");
                        return View(model);
                    }

                    var employee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.FullName == model.EmployeeName);
                    if (employee == null)
                    {
                        ModelState.AddModelError("EmployeeName", "Invalid employee name.");
                        return View(model);
                    }

                    // Update the order properties
                    order.OrderDate = model.OrderDate;
                    order.CompletionDate = model.CompletionDate;
                    order.CustomerId = customer.CustomerId;
                    order.EmployeeId = employee.EmployeeId;
                    order.Prepayment = model.Prepayment;
                    order.PaymentStatus = model.PaymentStatus;
                    order.CompletionStatus = model.CompletionStatus;
                    order.TotalCost = model.TotalCost;
                    order.WarrantyPeriod = model.WarrantyPeriod;

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Orders.Any(o => o.OrderId == model.OrderId))
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

            return View(model);
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
