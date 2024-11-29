using CyberCareServices.Controllers;
using CyberCareServices.Data;
using CyberCareServices.Model;
using CyberCareServices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ControllersTesting
{
    public class ServicesControllerTests
    {
        private readonly CyberCareServicesContext _context;
        private readonly ServicesController _controller;

        public ServicesControllerTests()
        {
            // Настроим In-Memory Database
            var options = new DbContextOptionsBuilder<CyberCareServicesContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CyberCareServicesContext(options);

            // Инициализируем контроллер с реальным контекстом
            _controller = new ServicesController(_context);
        }

        // Очищаем базу данных после каждого теста
        private async Task ClearDatabase()
        {
            _context.Services.RemoveRange(_context.Services);
            await _context.SaveChangesAsync();
        }

        #region Index Tests

        [Fact]
        public async Task Index_Returns_View_With_Services()
        {
            // Arrange: добавляем тестовые данные
            await ClearDatabase(); // Очистить базу
            _context.Services.AddRange(new List<Service>
            {
                new Service { ServiceId = 1, Name = "Service 1", Description = "Description 1", Cost = 100 },
                new Service { ServiceId = 2, Name = "Service 2", Description = "Description 2", Cost = 200 },
                new Service { ServiceId = 3, Name = "Service 3", Description = "Description 3", Cost = 300 }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ServicesViewModel>(viewResult.Model);
            Assert.Equal(3, model.Services.Count());
        }

        [Fact]
        public async Task Index_Returns_Empty_View_When_No_Services()
        {
            // Arrange: очищаем базу данных перед тестом
            await ClearDatabase();

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ServicesViewModel>(viewResult.Model);
            Assert.Empty(model.Services);
        }

        #endregion

        #region Details Tests

        [Fact]
        public async Task Details_Returns_View_With_Service()
        {
            // Arrange: добавляем данные
            await ClearDatabase();
            var service = new Service { ServiceId = 1, Name = "Service 1", Description = "Description 1", Cost = 100 };
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Service>(viewResult.Model);
            Assert.Equal(1, model.ServiceId);
        }

        [Fact]
        public async Task Details_Returns_NotFound_When_Service_Not_Exist()
        {
            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async Task Create_Returns_RedirectToAction_When_Model_Is_Valid()
        {
            // Arrange: создаём новый сервис
            var newService = new Service { Name = "New Service", Description = "New Description", Cost = 500 };

            // Act
            var result = await _controller.Create(newService);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Проверяем, что новый сервис был добавлен в базу
            var serviceInDb = await _context.Services.FirstOrDefaultAsync(s => s.Name == "New Service");
            Assert.NotNull(serviceInDb);
        }

        [Fact]
        public async Task Create_Returns_View_When_Model_Is_Invalid()
        {
            // Arrange: создаём сервис с ошибкой в модели (например, пустое имя)
            var newService = new Service { Name = "", Description = "Invalid Service", Cost = -100 };

            // Добавляем ошибку в ModelState
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(newService);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Service>(viewResult.Model);
            Assert.Equal("Invalid Service", model.Description);
        }

        #endregion

        #region Edit Tests

        [Fact]
        public async Task Edit_Successfully_Updates_Service()
        {
            // Arrange: добавляем сервис для редактирования
            await ClearDatabase();
            var serviceToUpdate = new Service { ServiceId = 1, Name = "Service 1", Description = "Description 1", Cost = 100 };
            _context.Services.Add(serviceToUpdate);
            await _context.SaveChangesAsync();

            // Изменяем имя сервиса
            serviceToUpdate.Name = "Updated Service";

            // Act
            var result = await _controller.Edit(1, serviceToUpdate);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Проверяем, что имя было обновлено
            var updatedService = await _context.Services.FindAsync(1);
            Assert.Equal("Updated Service", updatedService.Name);
        }

        [Fact]
        public async Task Edit_Returns_NotFound_When_Service_Not_Exist()
        {
            // Arrange: создаём новый сервис с несуществующим ID
            var serviceToUpdate = new Service { ServiceId = 999, Name = "Non-Existing Service" };

            // Act
            var result = await _controller.Edit(999, serviceToUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_Confirmed_Deletes_Service()
        {
            // Arrange: добавляем сервис для удаления
            await ClearDatabase();
            var serviceToDelete = new Service { ServiceId = 1, Name = "Service to Delete", Description = "To be deleted", Cost = 500 };
            _context.Services.Add(serviceToDelete);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Проверяем, что сервис был удалён
            var deletedService = await _context.Services.FindAsync(1);
            Assert.Null(deletedService);
        }

        [Fact]
        public async Task Delete_Returns_NotFound_When_Service_Not_Exist()
        {
            // Act
            var result = await _controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion
    }
}
