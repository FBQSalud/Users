using Application.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Presentation.Controllers;

namespace Tests.Presentation
{
    /// <summary>
    /// Unit tests for ProductoController.
    /// </summary>
    [TestFixture]
    public class ProductoControllerTests
    {
        private ProductoController _productoController;
        private Mock<IProductService> _mockProductService;
        private int _productId;
        private List<ProductoFindDto> _products;

        [SetUp]
        public void SetUp()
        {
            _mockProductService = new Mock<IProductService>();
            _productoController = new ProductoController(_mockProductService.Object);
            _productId = 1;

            _products = new List<ProductoFindDto>
        {
            new ProductoFindDto { ProductoId = 1, Nombre = "Product1" },
            new ProductoFindDto { ProductoId = 2, Nombre = "Product2" }
        };
        }

        [Test]
        public async Task GetProduct_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var product = new ProductoFindDto { ProductoId = _productId, Nombre = "Product1" };
            _mockProductService.Setup(service => service.FindProductById(_productId)).ReturnsAsync(product);

            // Act
            var result = await _productoController.GetProduct(_productId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(product));
        }

        [Test]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.FindProductById(_productId)).ReturnsAsync((ProductoFindDto)null);

            // Act
            var result = await _productoController.GetProduct(_productId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            var error = notFoundResult.Value as ErrorDto;
            Assert.Multiple(() =>
            {
                Assert.That(error.message, Is.EqualTo("Producto no encontrado"));
                Assert.That(error.statuscode, Is.EqualTo("404"));
            });
        }

        [Test]
        public async Task GetProductos_ShouldReturnOk_WhenProductsExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProducts(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>()))
                .ReturnsAsync(_products);

            // Act
            var result = await _productoController.GetProductos();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(_products));
        }

        [Test]
        public async Task GetProductos_ShouldReturnNotFound_WhenNoProductsFound()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProducts(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>()))
                .ThrowsAsync(new Exception("No products found"));

            // Act
            var result = await _productoController.GetProductos();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            var error = notFoundResult.Value as ErrorDto;
            Assert.Multiple(() =>
            {
                Assert.That(error.message, Is.EqualTo("No products found"));
                Assert.That(error.statuscode, Is.EqualTo("404"));
            });
        }

        [Test]
        public async Task GetProductosByCategoryOrBrand_ShouldReturnOk_WhenProductsExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductosByCategoryOrBrand(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(_products);

            // Act
            var result = await _productoController.GetProductosByCategoryOrBrand("Category1", null);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(_products));
        }

        [Test]
        public async Task GetProductosByCategoryOrBrand_ShouldReturnNotFound_WhenNoProductsFound()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductosByCategoryOrBrand(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("No products found"));

            // Act
            var result = await _productoController.GetProductosByCategoryOrBrand("Category1", "Brand1");

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            var error = notFoundResult.Value as ErrorDto;
            Assert.Multiple(() =>
            {
                Assert.That(error.message, Is.EqualTo("No products found"));
                Assert.That(error.statuscode, Is.EqualTo("404"));
            });
        }
    }
}
