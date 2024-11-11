using System;
using System.Collections.Generic;

namespace CyberCareServices.Model;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly? CompletionDate { get; set; }

    public int CustomerId { get; set; }

    public decimal? Prepayment { get; set; }

    public bool PaymentStatus { get; set; }

    public bool CompletionStatus { get; set; }

    public decimal TotalCost { get; set; }

    public int? WarrantyPeriod { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
