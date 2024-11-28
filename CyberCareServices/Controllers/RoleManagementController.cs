using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CyberCareServices.Areas.Identity.Models;
using CyberCareServices.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CyberCareServices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Метод для отображения всех пользователей и ролей
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var rolesForUser = new List<string>();
                foreach (var role in roles)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        rolesForUser.Add(role.Name);
                    }
                }

                userRoles.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = rolesForUser
                });
            }

            var viewModel = new RoleManagementViewModel
            {
                UserRoles = userRoles,
                Roles = roles
            };

            return View(viewModel);
        }


        // Метод для добавления роли пользователю
        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
            return RedirectToAction("Index");
        }

        // Метод для удаления роли у пользователя
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }
            return RedirectToAction("Index");
        }
    }
}
