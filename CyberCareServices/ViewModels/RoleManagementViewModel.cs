using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CyberCareServices.ViewModels
{
    public class RoleManagementViewModel
    {
        public List<UserViewModel> UserRoles { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}
