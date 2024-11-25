using Microsoft.AspNetCore.Mvc.Rendering;

namespace CyberCareServices.ViewModels
{
    public class AddComponentViewModel
    {
        public int OrderId { get; set; }
        public int ComponentId { get; set; }
        public SelectList AvailableComponents { get; set; }
    }

}
