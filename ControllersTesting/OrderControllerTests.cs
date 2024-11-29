using CyberCareServices.Controllers;
using CyberCareServices.Data;
using CyberCareServices.Model;
using CyberCareServices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ControllersTesting
{
    public class OrdersControllerTests
    {
        private CyberCareServicesContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<CyberCareServicesContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new CyberCareServicesContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task Details_Returns_NotFound_When_Order_Does_Not_Exist()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var controller = new OrdersController(context);

            // Act
            var result = await controller.Details(99); // No order with ID 99

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_Returns_View_When_Model_Is_Invalid()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var controller = new OrdersController(context);
            var invalidOrder = new OrderEditViewModel(); // Invalid because TotalCost is required
            controller.ModelState.AddModelError("TotalCost", "TotalCost is required.");

            // Act
            var result = await controller.Create(invalidOrder);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidOrder, viewResult.Model);
        }

        [Fact]
        public async Task Edit_Returns_NotFound_When_Order_Does_Not_Exist()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var controller = new OrdersController(context);

            // Act
            var result = await controller.Edit(99); // No order with ID 99

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Returns_NotFound_When_Order_Does_Not_Exist()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var controller = new OrdersController(context);

            // Act
            var result = await controller.Delete(99); // No order with ID 99

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
