using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace CyberCareServices.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Date of registration")]
        [DisplayFormat(DataFormatString = "0:dd-MM-yy", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }
    }
}
