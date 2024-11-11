using System;
using System.Collections.Generic;

namespace CyberCareServices.Model;

public partial class ComponentType
{
    public int ComponentTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();
}
