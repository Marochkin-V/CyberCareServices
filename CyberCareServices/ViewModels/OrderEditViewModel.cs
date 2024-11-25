using CyberCareServices.Model;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.ViewModels
{
    public class OrderEditViewModel
    {
        public List<Customer> Customers { get; set; }
        public List<Employee> Employees { get; set; }

        [Display(Name = "#")]
        public int OrderId { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        public DateOnly OrderDate { get; set; }

        [Display(Name = "Completion Date")]
        [DataType(DataType.Date)]
        public DateOnly? CompletionDate { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "Prepayment")]
        [DataType(DataType.Currency)]
        public decimal? Prepayment { get; set; }

        [Display(Name = "Payment Status")]
        public bool PaymentStatus { get; set; }

        [Display(Name = "Completion Status")]
        public bool CompletionStatus { get; set; }

        [Display(Name = "Total Cost")]
        [DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }

        [Display(Name = "Warranty Period (months)")]
        public int? WarrantyPeriod { get; set; }

        [Display(Name = "Employee")]
        public string? EmployeeName { get; set; }

        public virtual List<ComponentViewModel> Components { get; set; } = new List<ComponentViewModel>();

        public virtual List<Service> Services { get; set; } = new List<Service>();
    }
}
