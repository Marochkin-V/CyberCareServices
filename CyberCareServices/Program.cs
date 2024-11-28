//#define InitializationNeeded

using CyberCareServices.Areas.Identity.Models;
using CyberCareServices.Data;
using CyberCareServices.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CyberCareServices
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<CyberCareServicesContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() // Добавляем поддержку ролей
                .AddEntityFrameworkStores<CyberCareServicesContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession();

            var app = builder.Build();

            // Добавляем роли и администратора при запуске
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    await InitializeRolesAndAdminAsync(roleManager, userManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка инициализации ролей и администратора: {ex.Message}");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

#if InitializationNeeded
            app.UseDbInitializer();
#endif

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static async Task InitializeRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            // Определяем роли
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Создаём администратора
            var adminEmail = "admin@admin.com";
            var adminPassword = "Admin123!";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("Администратор создан и назначена роль 'Admin'.");
                }
                else
                {
                    Console.WriteLine("Ошибка создания администратора:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($" - {error.Description}");
                    }
                }
            }

            // Создаём обычного пользователя
            var userEmail = "user@user.com";
            var userPassword = "User123!";
            var regularUser = await userManager.FindByEmailAsync(userEmail);
            if (regularUser == null)
            {
                regularUser = new ApplicationUser { UserName = userEmail, Email = userEmail };
                var result = await userManager.CreateAsync(regularUser, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                    Console.WriteLine("Обычный пользователь создан и назначена роль 'User'.");
                }
                else
                {
                    Console.WriteLine("Ошибка создания пользователя:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($" - {error.Description}");
                    }
                }
            }
        }
    }
}
