using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.Model;

public partial class Order
{
    [Display(Name = "#")]
    public int OrderId { get; set; }

    [Display(Name = "Order Date")]
    [DataType(DataType.Date)]
    public DateOnly OrderDate { get; set; }

    [Display(Name = "Completion Date")]
    [DataType(DataType.Date)]
    public DateOnly? CompletionDate { get; set; }

    [Display(Name = "Customer ID")]
    public int CustomerId { get; set; }

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

    [Display(Name = "Employee ID")]
    public int? EmployeeId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
