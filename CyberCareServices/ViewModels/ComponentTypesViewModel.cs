using CyberCareServices.Model;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.ViewModels
{
    public class ComponentTypesViewModel
    {
        public List<ComponentType> ComponentTypes { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
    }

}
