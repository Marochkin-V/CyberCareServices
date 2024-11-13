using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.Model;

public partial class ComponentType
{
    [Display(Name = "#")]
    public int ComponentTypeId { get; set; }

    [Display(Name = "Название")]
    public string Name { get; set; } = null!;

    [Display(Name = "Описание")]
    public string? Description { get; set; }

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();
}
