using Azure;
using CyberCareServices.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CyberCareServices.ViewModels
{
    public class ComponentsViewModel
    {
        public IEnumerable<ComponentViewModel> Components { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}
