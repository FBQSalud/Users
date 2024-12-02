using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.model;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Presentation.Controllers;

namespace Tests.Presentation
{
    // <summary>
    /// Unit tests for CarritoController.
    /// </summary>
    [TestFixture]
    public class CarritoControllerTests
    {
        private CarritoController _carritoController;
        private Mock<ICarritoService> _mockCarritoService;
        private Mock<IMapper> _mockMapper;
        private Guid _carritoId;
        private int _clienteId;
        private int _productoid;
        private CarritoDto _carritoDto;
        private Carrito _carrito;
        private CompraCarritoDto _compraCarritoDto;

        /// <summary>
        /// Sets up the mock services and initializes test data for each test method.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockCarritoService = new Mock<ICarritoService>();
            _mockMapper = new Mock<IMapper>();
            _carritoController = new CarritoController(_mockCarritoService.Object, _mockMapper.Object);
            _carritoId = Guid.NewGuid();
            _clienteId = 1;
            _carrito = new Carrito { CarritoId = _carritoId, ClienteId = _clienteId, CarritoProductos = new List<CarritoProducto>() };
            _carritoDto = new CarritoDto { CarritoId = _carritoId, ClienteId = _clienteId, CarritoProductos = new List<CarritoProductoDto>() };
            _compraCarritoDto = new CompraCarritoDto { ClienteId = _clienteId, ProductoId = 2, Cantidad = 1 };
            _productoid = 2;
        }

        /// <summary>
        /// Tests that GetCarritoById returns OK with CarritoDto when Carrito exists.
        /// </summary>
        [Test]
        public async Task GetCarritoById_ShouldReturnOk_WhenCarritoExists()
        {
            _mockCarritoService.Setup(service => service.GetCarritoByClientId(_clienteId)).ReturnsAsync(_carrito);
            _mockMapper.Setup(mapper => mapper.Map<CarritoDto>(_carrito)).Returns(_carritoDto);

            var result = await _carritoController.GetCarritoById(_clienteId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(_carritoDto));
        }

        /// <summary>
        /// Tests that GetCarritoById returns NotFound when Carrito does not exist.
        /// </summary>
        [Test]
        public async Task GetCarritoById_ShouldReturnNotFound_WhenCarritoDoesNotExist()
        {
            _mockCarritoService.Setup(service => service.GetCarritoByClientId(_clienteId)).ReturnsAsync((Carrito)null);

            var result = await _carritoController.GetCarritoById(_clienteId);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.Value, Is.EqualTo("No se encontró un carrito con el Id provisto."));
        }

        /// <summary>
        /// Tests that AddProducto returns NoContent when product is added successfully.
        /// </summary>
        [Test]
        public async Task AddProducto_ShouldReturnNoContent_WhenProductIsAddedSuccessfully()
        {
            _mockCarritoService.Setup(service => service.AddCarritoProducto(_compraCarritoDto)).Returns(Task.FromResult(new Carrito()));

            var result = await _carritoController.AddProducto(_compraCarritoDto);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        /// <summary>
        /// Tests that AddProducto returns BadRequest when the product is invalid.
        /// </summary>
        [Test]
        public async Task AddProducto_ShouldReturnBadRequest_WhenProductIsInvalid()
        {
            CompraCarritoDto producto = null;

            var result = await _carritoController.AddProducto(producto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests that AddProducto returns NotFound when Carrito is not found.
        /// </summary>
        [Test]
        public async Task AddProducto_ShouldReturnNotFound_WhenCarritoNotFound()
        {
            _mockCarritoService.Setup(service => service.AddCarritoProducto(_compraCarritoDto)).ThrowsAsync(new ArgumentException("Carrito no encontrado"));

            var result = await _carritoController.AddProducto(_compraCarritoDto);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests that AddProducto returns Conflict when there is a failure in the operation.
        /// </summary>
        [Test]
        public async Task AddProducto_ShouldReturnConflict_WhenOperationFails()
        {
            _mockCarritoService.Setup(service => service.AddCarritoProducto(_compraCarritoDto)).ThrowsAsync(new InvalidOperationException("No se puede agregar el producto"));

            var result = await _carritoController.AddProducto(_compraCarritoDto);

            Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
        }

        /// <summary>
        /// Tests that ModifyProduct returns NoContent when product is modified successfully.
        /// </summary>
        [Test]
        public async Task ModifyProduct_ShouldReturnNoContent_WhenProductIsModifiedSuccessfully()
        {
            _mockCarritoService.Setup(service => service.ModifyProductInCarrito(_compraCarritoDto)).ReturnsAsync(new Carrito());

            var result = await _carritoController.ModifyProduct(_compraCarritoDto);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        /// <summary>
        /// Tests that ModifyProduct returns Conflict when modification fails.
        /// </summary>
        [Test]
        public async Task ModifyProduct_ShouldReturnConflict_WhenModifyFails()
        {
            _mockCarritoService.Setup(service => service.ModifyProductInCarrito(_compraCarritoDto)).ReturnsAsync((Carrito)null);

            var result = await _carritoController.ModifyProduct(_compraCarritoDto);

            Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
        }

        /// <summary>
        /// Tests that ModifyProduct returns BadRequest when the product is invalid.
        /// </summary>
        [Test]
        public async Task ModifyProduct_ShouldReturnBadRequest_WhenProductIsInvalid()
        {
            CompraCarritoDto producto = null;

            var result = await _carritoController.ModifyProduct(producto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Tests that DeleteProducto returns NoContent when product is deleted successfully.
        /// </summary>
        [Test]
        public async Task DeleteProducto_ShouldReturnNoContent_WhenProductIsDeletedSuccessfully()
        {
            _mockCarritoService.Setup(service => service.DeleteProductoFromCarrito(_clienteId, _productoid)).Returns(Task.FromResult(new Carrito()));

            var result = await _carritoController.DeleteProducto(_clienteId, _productoid);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        /// <summary>
        /// Tests that DeleteProducto returns NotFound when product is not found.
        /// </summary>
        [Test]
        public async Task DeleteProducto_ShouldReturnNotFound_WhenProductNotFound()
        {
            _mockCarritoService.Setup(service => service.DeleteProductoFromCarrito(_clienteId, _productoid)).ThrowsAsync(new ArgumentException("Producto no encontrado"));

            var result = await _carritoController.DeleteProducto(_clienteId, _productoid);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests that DeleteProducto returns Conflict when there is a failure in the deletion operation.
        /// </summary>
        [Test]
        public async Task DeleteProducto_ShouldReturnConflict_WhenOperationFails()
        {
            _mockCarritoService.Setup(service => service.DeleteProductoFromCarrito(_clienteId, _productoid)).ThrowsAsync(new InvalidOperationException("No se puede eliminar el producto"));

            var result = await _carritoController.DeleteProducto(_clienteId, _productoid);

            Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
        }

        /// <summary>
        /// Tests that DeleteCarrito returns NoContent when Carrito is deleted successfully.
        /// </summary>
        [Test]
        public async Task DeleteCarrito_ShouldReturnNoContent_WhenCarritoIsDeletedSuccessfully()
        {
            _mockCarritoService.Setup(service => service.DeleteCarrito(_carritoId)).ReturnsAsync(new Carrito());

            var result = await _carritoController.DeleteCarrito(_carritoId);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        /// <summary>
        /// Tests that DeleteCarrito returns NotFound when Carrito is not found.
        /// </summary>
        [Test]
        public async Task DeleteCarrito_ShouldReturnNotFound_WhenCarritoNotFound()
        {
            _mockCarritoService.Setup(service => service.DeleteCarrito(_carritoId)).ThrowsAsync(new FileNotFoundException("Carrito no encontrado"));

            var result = await _carritoController.DeleteCarrito(_carritoId);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests that DeleteCarrito returns InternalServerError when an unexpected error occurs during the deletion.
        /// </summary>
        [Test]
        public async Task DeleteCarrito_ShouldReturnInternalServerError_WhenAnErrorOccurs()
        {
            _mockCarritoService.Setup(service => service.DeleteCarrito(_carritoId)).ThrowsAsync(new Exception("Error inesperado"));

            var result = await _carritoController.DeleteCarrito(_carritoId);

            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        }
    }
}
