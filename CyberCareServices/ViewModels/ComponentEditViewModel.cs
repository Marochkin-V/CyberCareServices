using Azure;
using CyberCareServices.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.ViewModels
{
    public class ComponentEditViewModel
    {
        public int ComponentId { get; set; }

        [Display(Name = "ID компонента")]
        public string ComponentType { get; set; }

        [Display(Name = "Бренд")]
        public string Brand { get; set; } = null!;

        [Display(Name = "Производитель")]
        public string? Manufacturer { get; set; }

        [Display(Name = "Страна")]
        public string? CountryOfOrigin { get; set; }

        [Display(Name = "Дата выхода")]
        public DateOnly? ReleaseDate { get; set; }

        [Display(Name = "Спецификация")]
        public string? Specifications { get; set; }

        [Display(Name = "Гарантийный период")]
        public int? WarrantyPeriod { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        public List<ComponentType> ComponentTypes { get; set; }
    }
}
