using System;
using System.Collections.Generic;

namespace CyberCareServices.Model;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public bool DiscountAvailable { get; set; }

    public decimal? DiscountAmount { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
