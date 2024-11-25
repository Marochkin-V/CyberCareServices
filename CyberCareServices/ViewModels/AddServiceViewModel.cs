using Microsoft.AspNetCore.Mvc.Rendering;

namespace CyberCareServices.ViewModels
{
    public class AddServiceViewModel
    {
        public int OrderId { get; set; }
        public int ServiceId { get; set; }
        public SelectList AvailableServices { get; set; }
    }

}
