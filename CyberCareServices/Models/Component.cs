using System;
using System.Collections.Generic;

namespace CyberCareServices.Model;

public partial class Component
{
    public int ComponentId { get; set; }

    public int ComponentTypeId { get; set; }

    public string Brand { get; set; } = null!;

    public string? Manufacturer { get; set; }

    public string? CountryOfOrigin { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? Specifications { get; set; }

    public int? WarrantyPeriod { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual ComponentType ComponentType { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
