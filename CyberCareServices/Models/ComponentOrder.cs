using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberCareServices.Model;

public class ComponentOrder
{
    [Key, Column(Order = 0)]
    public int OrderId { get; set; }

    [Key, Column(Order = 1)]
    public int ComponentId { get; set; }

    public virtual Order Order { get; set; } = null!;
    public virtual Component Component { get; set; } = null!;
}
