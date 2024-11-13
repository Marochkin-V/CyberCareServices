using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.Model;

public partial class Customer
{
    [Display(Name = "#")]
    public int CustomerId { get; set; }

    [Display(Name = "Full Name")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "Phone Number")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = null!;

    [Display(Name = "Discount Available")]
    public bool DiscountAvailable { get; set; }

    [Display(Name = "Discount Amount (%)")]
    [Range(0, 100, ErrorMessage = "Discount amount must be between 0 and 100.")]
    public decimal? DiscountAmount { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
