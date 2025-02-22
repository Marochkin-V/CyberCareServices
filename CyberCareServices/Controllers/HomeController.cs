using CyberCareServices.Data;
using CyberCareServices.Models;
using CyberCareServices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CyberCareServices.ViewModels;

namespace CyberCareServices.Controllers
{
    public class HomeController : Controller
    {
        private readonly CyberCareServicesContext _context;

        public HomeController(CyberCareServicesContext context)
        {
            _context = context;
        }

        [ResponseCache(Duration = 2 * 5 + 240, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Index()
        {
            int rowsCount = 5;
            var viewModel = new HomeViewModel
            {
                ComponentTypes = _context.ComponentTypes.Take(rowsCount),
                Customers = _context.Customers.Take(rowsCount),
                Employees = _context.Employees.Take(rowsCount),
                Services = _context.Services.Take(rowsCount),
                Components = [.. _context.Components.OrderByDescending(c => c.ReleaseDate)
                    .Select(c => new ComponentViewModel{
                        ComponentId = c.ComponentId,
                        ComponentType = _context.ComponentTypes.FirstOrDefault(ct => ct.ComponentTypeId == c.ComponentTypeId).Name ?? "Component Type is not found",
                        Brand = c.Brand,
                        Manufacturer = c.Manufacturer,
                        CountryOfOrigin = c.CountryOfOrigin,
                        ReleaseDate = c.ReleaseDate,
                        Specifications = c.Specifications,
                        WarrantyPeriod = c.WarrantyPeriod,
                        Description = c.Description,
                        Price = c.Price,
                    })
                    .Take(rowsCount)],
                Orders = [.. _context.Orders
                    .Select(o => new OrderViewModel{
                        OrderId = o.OrderId,
                        OrderDate = o.OrderDate,
                        CompletionDate = o.CompletionDate,
                        CustomerName = _context.Customers.FirstOrDefault(c => c.CustomerId == o.CustomerId).FullName ?? "Customer is not found",
                        Prepayment = o.Prepayment,
                        PaymentStatus = o.PaymentStatus,
                        CompletionStatus = o.CompletionStatus,
                        TotalCost = o.TotalCost,
                        WarrantyPeriod = o.WarrantyPeriod,
                        EmployeeName = _context.Employees.FirstOrDefault(e => e.EmployeeId == o.EmployeeId).FullName ?? "Employee is not found",
                })
                    .Take(rowsCount)]
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
