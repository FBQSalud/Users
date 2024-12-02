using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Moq;
using NUnit.Framework;
using Presentation.Controllers;

namespace Tests.Presentation
{
    /// <summary>
    /// Unit tests for OrdenController.
    /// </summary>
    [TestFixture]
    public class OrdenControllerTests
    {
        private OrdenController _ordenController;
        private Mock<IOrdenService> _ordenService;
        private int _clienteid;

        [SetUp]
        public void SetUp()
        {
            _ordenService = new Mock<IOrdenService>();
            _ordenController = new OrdenController(_ordenService.Object);
            _clienteid = 1;
        }

        [Test]
        public async Task AddOrden_ShouldReturnOk_WhenOrdenIsCreatedSuccessfully()
        {
            // Arrange

            var orden = new Orden { OrdenId = Guid.NewGuid() };

            _ordenService
                .Setup(service => service.AddOrden(It.IsAny<int>()))
                .ReturnsAsync(orden);

            // Act
            var result = await _ordenController.AddOrden(_clienteid);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult is not null);

            dynamic response = okResult.Value;
            Assert.That(okResult.StatusCode == 200);
        }

        [Test]
        public async Task AddOrden_ShouldReturnNotFound_WhenClientIdDoesNotExist()
        {
            // Arrange
            _ordenService
                .Setup(service => service.AddOrden(_clienteid))
                .ThrowsAsync(new ArgumentException("El Id del cliente ingresado no existe."));

            // Act
            var result = await _ordenController.AddOrden(_clienteid);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult is not null);

            var error = notFoundResult.Value as ErrorDto;
            Assert.Multiple(() =>
            {
                Assert.That(error, Is.Not.EqualTo(null));
                Assert.That(error.statuscode, Is.EqualTo("404"));
                Assert.That(error.message, Is.EqualTo("El Id del cliente ingresado no existe."));
            });
        }

        [Test]
        public async Task AddOrden_ShouldReturnConflict_WhenCarritoIsEmpty()
        {
            // Arrange

            _ordenService
                .Setup(service => service.AddOrden(_clienteid))
                .ThrowsAsync(new InvalidOperationException("El carrito está vacío."));

            // Act
            var result = await _ordenController.AddOrden(_clienteid);

            // Assert
            var conflictResult = result as ConflictObjectResult;
            Assert.That(conflictResult is not null);

            var error = conflictResult.Value as ErrorDto;
            Assert.Multiple(() =>
            {
                Assert.That(error, Is.Not.EqualTo(null));
                Assert.That(error.statuscode, Is.EqualTo("409"));
                Assert.That(error.message, Is.EqualTo("El carrito está vacío."));
            });
        }

        [Test]
        public async Task AddOrden_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            _ordenService
                .Setup(service => service.AddOrden(_clienteid))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _ordenController.AddOrden(_clienteid);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.That(objectResult is not null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

            var error = objectResult.Value as ErrorDto;
            Assert.Multiple(() =>
            {
                Assert.That(error, Is.Not.EqualTo(null));
                Assert.That(error.statuscode, Is.EqualTo("500"));
                Assert.That(error.message, Is.EqualTo("Ocurrió un error inesperado."));
                Assert.That(error.detail, Is.EqualTo("Unexpected error"));
            });
        }

        [Test]
        public async Task GetOrdenes_ShouldReturnNotFound_WhenNoOrdersExist()
        {
            // Arrange
            _ordenService
                .Setup(service => service.GetOrder(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .ReturnsAsync((OrdenResponse)null);

            // Act
            var result = await _ordenController.GetOrdenes();

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.EqualTo(null));

            var error = notFoundResult.Value as ErrorDto;
            Assert.That(error, Is.Not.EqualTo(null));
            Assert.Multiple(() =>
            {
                Assert.That(error.statuscode, Is.EqualTo("404"));
                Assert.That(error.message, Is.EqualTo("No se encontraron ordenes."));
            });
        }

    }
}