using CyberCareServices.Controllers;
using CyberCareServices.Data;
using CyberCareServices.Model;
using CyberCareServices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ControllersTesting
{
    public class ComponentTypesControllerTests
    {
        private readonly Mock<CyberCareServicesContext> _mockContext;
        private readonly ComponentTypesController _controller;

        public ComponentTypesControllerTests()
        {
            var options = new DbContextOptionsBuilder<CyberCareServicesContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _mockContext = new Mock<CyberCareServicesContext>(options);
            _controller = new ComponentTypesController(_mockContext.Object);
        }

        // Тест для метода Create (GET)
        [Fact]
        public void Create_Returns_View()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        // Тест для метода Create (POST)
        [Fact]
        public async Task Create_Post_Returns_Redirect_When_Valid()
        {
            // Arrange
            var componentType = new ComponentType { ComponentTypeId = 1, Name = "NewType", Description = "New Description" };
            _mockContext.Setup(m => m.Add(It.IsAny<ComponentType>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _controller.Create(componentType) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_Post_Returns_View_When_Invalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var componentType = new ComponentType { ComponentTypeId = 1, Name = "", Description = "New Description" };

            // Act
            var result = await _controller.Create(componentType) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as ComponentType;
            Assert.Equal(componentType, model);
        }

        // Тест для метода Edit (GET)
        [Fact]
        public async Task Edit_Returns_View_When_ComponentType_Exists()
        {
            // Arrange
            var componentType = new ComponentType { ComponentTypeId = 1, Name = "Type1", Description = "Description1" };
            _mockContext.Setup(m => m.ComponentTypes.FindAsync(1)).ReturnsAsync(componentType);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as ComponentType;
            Assert.NotNull(model);
            Assert.Equal("Type1", model.Name);
        }

        [Fact]
        public async Task Edit_Returns_NotFound_When_ComponentType_Does_Not_Exist()
        {
            // Arrange
            _mockContext.Setup(m => m.ComponentTypes.FindAsync(1)).ReturnsAsync((ComponentType)null);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Тест для метода Edit (POST)
        [Fact]
        public async Task Edit_Post_Returns_Redirect_When_Valid()
        {
            // Arrange
            var componentType = new ComponentType { ComponentTypeId = 1, Name = "UpdatedType", Description = "Updated Description" };
            _mockContext.Setup(m => m.Update(It.IsAny<ComponentType>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _controller.Edit(1, componentType) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_Post_Returns_View_When_Invalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var componentType = new ComponentType { ComponentTypeId = 1, Name = "", Description = "Updated Description" };

            // Act
            var result = await _controller.Edit(1, componentType) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as ComponentType;
            Assert.Equal(componentType, model);
        }

        // Тест для метода Delete (POST)
        [Fact]
        public async Task DeleteConfirmed_Removes_ComponentType_And_Redirects()
        {
            // Arrange
            var componentType = new ComponentType { ComponentTypeId = 1, Name = "Type1", Description = "Description1" };
            _mockContext.Setup(m => m.ComponentTypes.FindAsync(1)).ReturnsAsync(componentType);
            _mockContext.Setup(m => m.ComponentTypes.Remove(It.IsAny<ComponentType>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
