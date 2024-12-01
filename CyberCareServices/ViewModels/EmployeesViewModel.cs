using CyberCareServices.Model;

namespace CyberCareServices.ViewModels
{
    public class EmployeesViewModel
    {
        public List<Employee> Employees { get; set; }
        public string SearchQuery { get; set; } // Для хранения поискового запроса
        public string SortField { get; set; }
        public string SortOrder { get; set; }
    }
}
