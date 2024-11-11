using System;
using System.Collections.Generic;

namespace CyberCareServices.Model;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Position { get; set; }

    public DateOnly DateOfHire { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
