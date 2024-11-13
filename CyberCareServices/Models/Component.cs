using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.Model;

public partial class Component
{
    [Display(Name = "#")]
    public int ComponentId { get; set; }

    [Display(Name = "Component Type")]
    public int ComponentTypeId { get; set; }

    [Display(Name = "Brand")]
    public string Brand { get; set; } = null!;

    [Display(Name = "Manufacturer")]
    public string? Manufacturer { get; set; }

    [Display(Name = "Country of Origin")]
    public string? CountryOfOrigin { get; set; }

    [Display(Name = "Release Date")]
    public DateOnly? ReleaseDate { get; set; }

    [Display(Name = "Specifications")]
    public string? Specifications { get; set; }

    [Display(Name = "Warranty Period (months)")]
    public int? WarrantyPeriod { get; set; }

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Price")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    public virtual ComponentType ComponentType { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
