using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CyberCareServices.Controllers;
using CyberCareServices.Data;
using CyberCareServices.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using CyberCareServices.ViewModels;

namespace ControllersTesting
{
    public class ComponentControllerTests
    {
        private readonly Mock<CyberCareServicesContext> _mockContext;
        private readonly Mock<DbSet<Component>> _mockComponentSet;
        private readonly Mock<DbSet<ComponentType>> _mockComponentTypeSet;
        private readonly ComponentsController _controller;

        public ComponentControllerTests()
        {
            // Инициализируем mock контекст с использованием InMemoryDatabase
            var options = new DbContextOptionsBuilder<CyberCareServicesContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var context = new CyberCareServicesContext(options);
            _mockContext = new Mock<CyberCareServicesContext>(options);

            // Мокируем DbSet для Components
            _mockComponentSet = new Mock<DbSet<Component>>();
            _mockContext.Setup(c => c.Components).Returns(_mockComponentSet.Object);

            // Мокируем DbSet для ComponentTypes
            _mockComponentTypeSet = new Mock<DbSet<ComponentType>>();
            _mockContext.Setup(c => c.ComponentTypes).Returns(_mockComponentTypeSet.Object);

            _controller = new ComponentsController(context);
        }

        [Fact]
        public async Task Details_Returns_NotFound_When_Component_Does_Not_Exist()
        {
            // Arrange
            _mockComponentSet.Setup(m => m.FindAsync(1)).ReturnsAsync((Component)null);

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_Returns_View_When_Invalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("ComponentType", "Invalid type");
            var model = new ComponentViewModel();

            // Act
            var result = await _controller.Create(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task Edit_Returns_NotFound_When_Component_Does_Not_Exist()
        {
            // Arrange
            _mockContext.Setup(c => c.Components.FindAsync(1)).ReturnsAsync((Component)null);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Returns_NotFound_When_Component_Does_Not_Exist()
        {
            // Arrange
            _mockContext.Setup(c => c.Components.FindAsync(1)).ReturnsAsync((Component)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Тест для метода DeleteConfirmed
        [Fact]
        public async Task DeleteConfirmed_Removes_Component_And_Redirects()
        {
            // Arrange
            var component = new Component { ComponentId = 1 };
            _mockContext.Setup(c => c.Components.FindAsync(1)).ReturnsAsync(component);

            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
