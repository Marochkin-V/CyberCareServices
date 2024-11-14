using Azure;
using CyberCareServices.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CyberCareServices.ViewModels
{
    public class OrdersViewModel
    {
        public IEnumerable<OrderViewModel> Orders { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}
