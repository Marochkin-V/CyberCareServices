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
    public class CustomersControllerTests
    {
        private readonly Mock<CyberCareServicesContext> _mockContext;
        private readonly CustomersController _controller;
        private readonly int PageSize = 20;

        public CustomersControllerTests()
        {
            var options = new DbContextOptionsBuilder<CyberCareServicesContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _mockContext = new Mock<CyberCareServicesContext>(options);
            _controller = new CustomersController(_mockContext.Object);
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
            var customer = new Customer { FullName = "John Doe", Phone = "+1234567890", DiscountAvailable = true, DiscountAmount = 10 };
            _mockContext.Setup(m => m.Add(It.IsAny<Customer>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _controller.Create(customer) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_Post_Returns_View_When_Invalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("FullName", "Required");
            var customer = new Customer { FullName = "", Phone = "+1234567890", DiscountAvailable = true, DiscountAmount = 10 };

            // Act
            var result = await _controller.Create(customer) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as Customer;
            Assert.Equal(customer, model);
        }

        // Тесты для метода Edit (GET)
        [Fact]
        public async Task Edit_Returns_View_When_Customer_Exists()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, FullName = "John Doe", Phone = "+1234567890" };
            _mockContext.Setup(m => m.Customers.FindAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as Customer;
            Assert.NotNull(model);
            Assert.Equal("John Doe", model.FullName);
        }

        [Fact]
        public async Task Edit_Returns_NotFound_When_Customer_Does_Not_Exist()
        {
            // Arrange
            _mockContext.Setup(m => m.Customers.FindAsync(1)).ReturnsAsync((Customer)null);

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

        // Тесты для метода Edit (POST)
        [Fact]
        public async Task Edit_Post_Returns_Redirect_When_Valid()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, FullName = "John Doe", Phone = "+1234567890" };
            _mockContext.Setup(m => m.Update(It.IsAny<Customer>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _controller.Edit(1, customer) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_Post_Returns_View_When_Invalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("FullName", "Required");
            var customer = new Customer { CustomerId = 1, FullName = "", Phone = "+1234567890" };

            // Act
            var result = await _controller.Edit(1, customer) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as Customer;
            Assert.Equal(customer, model);
        }

        [Fact]
        public async Task Edit_Post_Returns_NotFound_When_Id_Mismatch()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, FullName = "John Doe", Phone = "+1234567890" };

            // Act
            var result = await _controller.Edit(2, customer);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Returns_NotFound_When_Id_Is_Null()
        {
            // Act
            var result = await _controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Тесты для метода Delete (POST)
        [Fact]
        public async Task DeleteConfirmed_Returns_Redirect_After_Delete()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, FullName = "John Doe", Phone = "+1234567890" };
            _mockContext.Setup(m => m.Customers.FindAsync(1)).ReturnsAsync(customer);
            _mockContext.Setup(m => m.Customers.Remove(It.IsAny<Customer>()));
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
