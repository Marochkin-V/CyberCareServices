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

        public IActionResult Index()
        {
            int rowsCount = 10;
            var viewModel = new HomeViewModel
            {
                ComponentTypes = _context.ComponentTypes.ToList(),
                Customers = _context.Customers.ToList(),
                Employees = _context.Employees.ToList(),
                Services = _context.Services.ToList(),
                Components = [.. _context.Components.OrderByDescending(c => c.ReleaseDate)
                    .Select(c => new ComponentsViewModel{
                        ComponentId = c.ComponentId,
                        ComponentType = _context.ComponentTypes.FirstOrDefault(ct => ct.ComponentTypeId == c.ComponentTypeId).Name,
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
                Orders = _context.Orders.ToList(),
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
