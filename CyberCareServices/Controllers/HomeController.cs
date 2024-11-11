using CyberCareServices.Data;
using CyberCareServices.Models;
using CyberCareServices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            var viewModel = new HomeViewModel
            {
                ComponentTypes = _context.ComponentTypes.ToList(),
                Components = _context.Components.ToList(),
                Customers = _context.Customers.ToList(),
                Employees = _context.Employees.ToList(),
                Services = _context.Services.ToList(),
                Orders = _context.Orders.ToList()
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
