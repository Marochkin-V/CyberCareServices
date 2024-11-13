using CyberCareServices.Model;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.ViewModels
{
    public class ComponentTypesViewModel
    {
        public IEnumerable<ComponentType> ComponentTypes { get; set; }
    }
}
