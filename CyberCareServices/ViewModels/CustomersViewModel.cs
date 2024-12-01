using CyberCareServices.Model;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.ViewModels
{
    public class CustomersViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string SearchQuery { get; set; }
    }
}
