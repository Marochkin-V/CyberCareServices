using CyberCareServices.Model;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.ViewModels
{
    public class ComponentTypeViewModel
    {
        public IEnumerable<ComponentType> ComponentTypes { get; set; }
    }
}
