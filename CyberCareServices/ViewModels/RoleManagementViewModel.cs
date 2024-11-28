using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CyberCareServices.ViewModels
{
    public class RoleManagementViewModel
    {
        public List<UserRoleViewModel> UserRoles { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }

    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
