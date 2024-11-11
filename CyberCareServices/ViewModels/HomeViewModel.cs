using CyberCareServices.Model;

namespace CyberCareServices.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ComponentType> ComponentTypes { get; set; }
        public IEnumerable<Component> Components { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
