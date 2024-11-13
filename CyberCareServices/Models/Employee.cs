using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.Model;

public partial class Employee
{
    [Display(Name = "#")]
    public int EmployeeId { get; set; }

    [Display(Name = "Full Name")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Position")]
    public string? Position { get; set; }

    [Display(Name = "Date of Hire")]
    [DataType(DataType.Date)]
    public DateOnly DateOfHire { get; set; }

    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }

    [Display(Name = "Phone Number")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = null!;

    [Display(Name = "Email Address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
