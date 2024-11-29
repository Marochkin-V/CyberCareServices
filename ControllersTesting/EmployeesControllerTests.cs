using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CyberCareServices.Controllers;
using CyberCareServices.Data;
using CyberCareServices.Model;
using CyberCareServices.ViewModels;
using Xunit;

namespace ControllersTesting
{
    public class EmployeesControllerTests
    {
        private readonly Mock<CyberCareServicesContext> _mockContext;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            var options = new DbContextOptionsBuilder<CyberCareServicesContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _mockContext = new Mock<CyberCareServicesContext>(options);
            _controller = new EmployeesController(_mockContext.Object);
        }

        [Fact]
        public async Task Details_Returns_NotFound_When_Id_Is_Null()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Тесты для метода Create (GET)
        [Fact]
        public void Create_Returns_View()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        // Тесты для метода Create (POST)
        [Fact]
        public async Task Create_Post_Returns_Redirect_When_Valid()
        {
            // Arrange
            var employee = new Employee { FullName = "John Doe", Position = "Developer", DateOfHire = new DateOnly(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day), DateOfBirth = new DateOnly(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day), Phone = "+1234567890", Email = "john.doe@example.com" };
            _mockContext.Setup(m => m.Add(It.IsAny<Employee>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _controller.Create(employee) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_Post_Returns_View_When_Invalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("FullName", "Required");
            var employee = new Employee { FullName = "", Position = "Developer", DateOfHire = new DateOnly(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day), DateOfBirth = new DateOnly(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day), Phone = "+1234567890", Email = "john.doe@example.com" };

            // Act
            var result = await _controller.Create(employee) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as Employee;
            Assert.Equal(employee, model);
        }

        // Тесты для метода Edit (GET)
        [Fact]
        public async Task Edit_Returns_View_When_Employee_Exists()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, FullName = "John Doe", Position = "Developer" };
            _mockContext.Setup(m => m.Employees.FindAsync(1)).ReturnsAsync(employee);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as Employee;
            Assert.NotNull(model);
            Assert.Equal("John Doe", model.FullName);
        }

        [Fact]
        public async Task Edit_Returns_NotFound_When_Employee_Not_Found()
        {
            // Arrange
            _mockContext.Setup(m => m.Employees.FindAsync(1)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Returns_NotFound_When_Id_Is_Null()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_Returns_View_When_Invalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("FullName", "Required");
            var employee = new Employee { EmployeeId = 1, FullName = "", Position = "Developer", DateOfHire = new DateOnly(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day), DateOfBirth = new DateOnly(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day), Phone = "+1234567890", Email = "john.doe@example.com" };

            // Act
            var result = await _controller.Edit(1, employee) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as Employee;
            Assert.Equal(employee, model);
        }

        [Fact]
        public async Task Edit_Post_Returns_NotFound_When_Id_Mismatch()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, FullName = "John Doe", Position = "Developer" };

            // Act
            var result = await _controller.Edit(2, employee);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Тесты для метода Delete (POST)
        [Fact]
        public async Task DeleteConfirmed_Returns_Redirect_After_Delete()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, FullName = "John Doe", Position = "Developer" };
            _mockContext.Setup(m => m.Employees.FindAsync(1)).ReturnsAsync(employee);
            _mockContext.Setup(m => m.Employees.Remove(It.IsAny<Employee>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
