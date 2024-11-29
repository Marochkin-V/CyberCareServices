using CyberCareServices.Controllers;
using CyberCareServices.Areas.Identity.Models;
using CyberCareServices.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CyberCareServices.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ControllersTesting
{
    public class UsersControllerTests
    {
        // Метод для создания InMemory контекста с Identity
        private UserManager<ApplicationUser> CreateUserManager()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var context = new ApplicationDbContext(options);
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore, null, new PasswordHasher<ApplicationUser>(), null, null, null, null, null, null);

            return userManager;
        }

        private RoleManager<IdentityRole> CreateRoleManager()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var context = new ApplicationDbContext(options);
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore, null, null, null, null);

            return roleManager;
        }

        // --- Тест для Index метода ---
        [Fact]
        public async Task Index_Returns_View_With_Users_And_Roles()
        {
            // Arrange
            var userManager = CreateUserManager();
            var roleManager = CreateRoleManager();
            var controller = new UsersController(userManager, roleManager);

            // Создание пользователей
            var user1 = new ApplicationUser { UserName = "user1", Email = "user1@example.com" };
            var user2 = new ApplicationUser { UserName = "user2", Email = "user2@example.com" };
            await userManager.CreateAsync(user1, "Password123!");
            await userManager.CreateAsync(user2, "Password123!");

            // Создание ролей
            var role1 = new IdentityRole("Admin");
            var role2 = new IdentityRole("User");
            await roleManager.CreateAsync(role1);
            await roleManager.CreateAsync(role2);

            // Присвоение ролей пользователям
            await userManager.AddToRoleAsync(user1, "Admin");
            await userManager.AddToRoleAsync(user2, "User");

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RoleManagementViewModel>(viewResult.Model);
            Assert.Equal(2, model.UserRoles.Count());
            Assert.Equal(2, model.Roles.Count());
        }

        // --- Тест для метода DeleteConfirmed ---
        [Fact]
        public async Task DeleteConfirmed_Deletes_User()
        {
            // Arrange
            var userManager = CreateUserManager();
            var roleManager = CreateRoleManager();
            var controller = new UsersController(userManager, roleManager);

            // Создание пользователя
            var user = new ApplicationUser { UserName = "user1", Email = "user1@example.com" };
            await userManager.CreateAsync(user, "Password123!");

            // Act
            var result = await controller.DeleteConfirmed(user.Id);

            // Assert
            var userInDb = await userManager.FindByIdAsync(user.Id);
            Assert.Null(userInDb); // Пользователь должен быть удалён
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        // --- Тест для метода Edit (GET) с существующим пользователем ---
        [Fact]
        public async Task Edit_Returns_View_With_User_Data()
        {
            // Arrange
            var userManager = CreateUserManager();
            var roleManager = CreateRoleManager();
            var controller = new UsersController(userManager, roleManager);

            var user = new ApplicationUser { UserName = "user1", Email = "user1@example.com" };
            await userManager.CreateAsync(user, "Password123!");

            // Act
            var result = await controller.Edit(user.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UserEditViewModel>(viewResult.Model);
            Assert.Equal(user.UserName, model.UserName);
            Assert.Equal(user.Email, model.Email);
        }

        // --- Тест для метода Edit (POST) с успешным обновлением данных ---
        [Fact]
        public async Task Edit_Post_Updates_User_Data()
        {
            // Arrange
            var userManager = CreateUserManager();
            var roleManager = CreateRoleManager();
            var controller = new UsersController(userManager, roleManager);

            var user = new ApplicationUser { UserName = "user1", Email = "user1@example.com" };
            await userManager.CreateAsync(user, "Password123!");

            var model = new UserEditViewModel { UserId = user.Id, UserName = "updatedUser", Email = "updated@example.com" };

            // Act
            var result = await controller.Edit(model);

            // Assert
            var updatedUser = await userManager.FindByIdAsync(user.Id);
            Assert.Equal("updatedUser", updatedUser.UserName);
            Assert.Equal("updated@example.com", updatedUser.Email);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        // --- Тест для метода Edit (POST) с ошибками валидации ---
        [Fact]
        public async Task Edit_Post_Returns_View_With_ModelState_Errors()
        {
            // Arrange
            var userManager = CreateUserManager();
            var roleManager = CreateRoleManager();
            var controller = new UsersController(userManager, roleManager);

            var user = new ApplicationUser { UserName = "user1", Email = "user1@example.com" };
            await userManager.CreateAsync(user, "Password123!");

            var model = new UserEditViewModel { UserId = user.Id, UserName = "", Email = "updated@example.com" }; // Невалидное имя

            controller.ModelState.AddModelError("UserName", "User name is required");

            // Act
            var result = await controller.Edit(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(user.Id, (viewResult.Model as UserEditViewModel)?.UserId);
            Assert.False(controller.ModelState.IsValid);
        }
    }
}
