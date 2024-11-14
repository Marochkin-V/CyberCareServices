﻿using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.ViewModels
{
    public class ServicesViewModel
    {
        [Display(Name = "#")]
        public int ServiceId { get; set; }

        [Display(Name = "Service Name")]
        [Required(ErrorMessage = "Service name is required.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Cost")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Cost must be a positive value.")]
        public decimal Cost { get; set; }
    }
}
