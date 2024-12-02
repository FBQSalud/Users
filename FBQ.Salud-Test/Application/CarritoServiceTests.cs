using Application.DataAccess;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Microsoft.OpenApi.Any;
using Moq;
using NUnit.Framework;

namespace Tests.Application;

/// <summary>
/// Unit tests for CarritoService.
/// </summary>
[TestFixture]
public class CarritoServiceTests
{
    private Mock<ICarritoRepository> _mockRepository;
    private CarritoService _carritoService;
    private Mock<IClienteService> _clienteService;
    private Mock<IProductService> _productService;

    /// <summary>
    /// Sets up the test environment by initializing mocks and the service.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<ICarritoRepository>();
        _clienteService = new Mock<IClienteService>();
        _productService = new Mock<IProductService>();
        _carritoService = new CarritoService(_mockRepository.Object,_clienteService.Object,_productService.Object);
    }

    /// <summary>
    /// Tests that GetCarritoByClientId returns the expected carrito when found.
    /// </summary>
    [Test]
    public async Task GetCarritoByClientId_ShouldReturnCarrito_WhenFound()
    {
        // Arrange
        int clientId = 1;
        var expectedCarrito = new Carrito { CarritoId = Guid.NewGuid(), ClienteId = clientId };
        _mockRepository.Setup(repo => repo.GetCarritoByClientId(clientId))
            .ReturnsAsync(expectedCarrito);

        // Act
        var result = await _carritoService.GetCarritoByClientId(clientId);

        // Assert
        Assert.That(result is not null);
        Assert.That(expectedCarrito == result);
        _mockRepository.Verify(repo => repo.GetCarritoByClientId(clientId), Times.Once);
    }


    /// <summary>
    /// Tests that GetRawCarritoById returns the expected carrito when found.
    /// </summary>
    [Test]
    public async Task GetRawCarritoByIdd_ShouldReturnCarrito_WhenFound()
    {
        // Arrange
        var carritoId = new Guid();
        var expectedCarrito = new Carrito { CarritoId = carritoId, };
        _mockRepository.Setup(repo => repo.GetRawCarritoById(carritoId))
            .ReturnsAsync(expectedCarrito);

        // Act
        var result = await _carritoService.GetRawCarritoById(carritoId);

        // Assert
        Assert.That(result is not null);
        Assert.That(expectedCarrito == result);
        _mockRepository.Verify(repo => repo.GetRawCarritoById(carritoId), Times.Once);
    }

   

    /// <summary>
    /// Tests that DeleteCarrito returns the deleted carrito when it exists.
    /// </summary>
    [Test]
    public async Task DeleteCarrito_ShouldReturnDeletedCarrito_WhenFound()
    {
        // Arrange
        var carritoId = Guid.NewGuid();
        var carrito = new Carrito { CarritoId = carritoId };
        _mockRepository.Setup(repo => repo.GetRawCarritoById(carritoId))
            .ReturnsAsync(carrito);
        _mockRepository.Setup(repo => repo.DeleteCarritoById(carritoId))
            .ReturnsAsync(carrito);

        // Act
        var result = await _carritoService.DeleteCarrito(carritoId);

        // Assert
        Assert.That(result is not null);
        Assert.That(carrito == result);
        _mockRepository.Verify(repo => repo.GetRawCarritoById(carritoId), Times.Once);
        _mockRepository.Verify(repo => repo.DeleteCarritoById(carritoId), Times.Once);
    }

    /// <summary>
    /// Tests that DeleteCarrito throws a FileNotFoundException when the carrito does not exist.
    /// </summary>
    [Test]
    public void DeleteCarrito_ShouldThrowFileNotFoundException_WhenCarritoNotFound()
    {
        // Arrange
        var carritoId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.GetRawCarritoById(carritoId))
            .ReturnsAsync((Carrito)null); // Simulate carrito not found

        // Act & Assert
        var ex = Assert.ThrowsAsync<FileNotFoundException>(async () =>
            await _carritoService.DeleteCarrito(carritoId));

        Assert.That(ex.Message, Is.EqualTo($"CarritoId : {carritoId} no encontrado"));
        _mockRepository.Verify(repo => repo.GetRawCarritoById(carritoId), Times.Once);
        _mockRepository.Verify(repo => repo.DeleteCarritoById(It.IsAny<Guid>()), Times.Never);
    }

    /// <summary>
    /// Tests that UpdateCarrito returns the updated carrito.
    /// </summary>
    [Test]
    public async Task UpdateCarrito_ShouldReturnUpdatedCarrito()
    {
        // Arrange
        var carrito = new Carrito { CarritoId = Guid.NewGuid() };
        _mockRepository.Setup(repo => repo.UpdateCarrito(carrito))
            .ReturnsAsync(carrito);

        // Act
        var result = await _carritoService.UpdateCarrito(carrito);

        // Assert
        Assert.That(result is not null);
        Assert.That(carrito == result);
        _mockRepository.Verify(repo => repo.UpdateCarrito(carrito), Times.Once);
    }



    /// <summary>
    /// Tests that ModifyProductInCarrito returns the carrito.
    /// </summary>
    [Test]
    public async Task ModifyProductInCarrito_ShouldReturnCarrito()
    {
        // Arrange
        int clienteId = 1;
        int productoId = 1;
        var carritoId = Guid.NewGuid();

        var compraCarritoDto = new CompraCarritoDto() { ClienteId = clienteId, Cantidad = 1 };
        var carritoProducto = new CarritoProducto() { CarritoId = carritoId , ProductoId = productoId };
        var cliente = new ClienteDto() { ClienteId = clienteId };
        var Producto = new Producto() {ProductoId = productoId, };
        var carrito = new Carrito { CarritoId = Guid.NewGuid(),ClienteId = clienteId};

        _mockRepository.Setup(repo => repo.GetCarritoByClientId(It.IsAny<int>()))
            .ReturnsAsync(carrito);
        _mockRepository.Setup(repo => repo.GetCarritoProductoById(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(carritoProducto);
        _productService.Setup(productService => productService.FindRawProductById(It.IsAny<int>()))
            .ReturnsAsync(Producto);
        _clienteService.Setup(clienteService => clienteService.GetClienteById(It.IsAny<int>()))
            .ReturnsAsync(cliente);
        _mockRepository.Setup(repo => repo.UpdateCarritoProducto(It.IsAny<CarritoProducto>()))
           .ReturnsAsync(carritoProducto);

        // Act
        var result = await _carritoService.ModifyProductInCarrito(compraCarritoDto);

        // Assert
        Assert.That(result is not null);
        Assert.That(carrito == result);
        _mockRepository.Verify(repo => repo.UpdateCarritoProducto(carritoProducto), Times.Once);
    }


    /// <summary>
    /// Tests that ModifyProductInCarrito throws InvalidOperationException.
    /// </summary>
    [Test]
    public async Task ModifyProductInCarrito_ShouldThrowInvalidOperationException()
    {
        // Arrange
        int clienteId = 1;
        int productoId = 1;
        var carritoId = Guid.NewGuid();

        var compraCarritoDto = new CompraCarritoDto() { ClienteId = clienteId, Cantidad = 0 };
        var carritoProducto = new CarritoProducto() { CarritoId = carritoId, ProductoId = productoId };
        var cliente = new ClienteDto() { ClienteId = clienteId };
        var Producto = new Producto() { ProductoId = productoId, };
        var carrito = new Carrito { CarritoId = Guid.NewGuid(), ClienteId = clienteId };

        _mockRepository.Setup(repo => repo.GetCarritoByClientId(It.IsAny<int>()))
            .ReturnsAsync(carrito);
        _mockRepository.Setup(repo => repo.GetCarritoProductoById(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(carritoProducto);
        _productService.Setup(productService => productService.FindRawProductById(It.IsAny<int>()))
            .ReturnsAsync(Producto);
        _clienteService.Setup(clienteService => clienteService.GetClienteById(It.IsAny<int>()))
            .ReturnsAsync(cliente);
        _mockRepository.Setup(repo => repo.UpdateCarritoProducto(It.IsAny<CarritoProducto>()))
           .ReturnsAsync(carritoProducto);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
           await _carritoService.ModifyProductInCarrito(compraCarritoDto));

        Assert.That(ex.Message, Is.EqualTo($"No se puede tener menos de un producto en el carrito."));
    }

    /// <summary>
    /// Tests that AddCarritoProducto returns carrito, uses createCarrito.
    /// </summary>
    [Test]
    public async Task AddCarritoProducto_ShouldReturnCarrito()
    {
        // Arrange
        int clienteId = 1;
        int productoId = 1;
        var carritoId = Guid.NewGuid();

        var compraCarritoDto = new CompraCarritoDto() { ClienteId = clienteId, Cantidad = 1, ProductoId = productoId };
        var carritoProducto = new CarritoProducto() { CarritoId = carritoId, ProductoId = productoId };
        var Producto = new Producto() { ProductoId = productoId, };
        var cliente = new ClienteDto() { ClienteId = clienteId };

        var carrito = new Carrito { CarritoId = Guid.NewGuid(), ClienteId = clienteId };
        _mockRepository.Setup(repo => repo.GetCarritoByClientId(clienteId))
            .ReturnsAsync((Carrito)null);

        _mockRepository.Setup(repo => repo.CreateCarrito(It.IsAny<Carrito>()))
          .ReturnsAsync(carrito);
        _clienteService.Setup(clienteService => clienteService.GetClienteById(clienteId))
         .ReturnsAsync(cliente);
        _productService.Setup(productService => productService.FindRawProductById(clienteId))
        .ReturnsAsync(Producto);

        // Act
        var result = await _carritoService.AddCarritoProducto(compraCarritoDto);

        // Assert
        Assert.That(result is not null);
        Assert.That(carrito == result);
        _mockRepository.Verify(repo => repo.CreateCarrito(It.IsAny<Carrito>()), Times.Once);
    }

    /// <summary>
    /// Tests that AddCarritoProducto returns carrito and uses updateCarrito.
    /// </summary>
    [Test]
    public async Task AddCarritoProducto_ShouldUpdateCarrito()
    {
        // Arrange
        int clienteId = 1;
        int productoId = 1;
        var carritoId = Guid.NewGuid();

        var compraCarritoDto = new CompraCarritoDto() { ClienteId = clienteId, Cantidad = 1, ProductoId = productoId };
        var carritoProducto = new CarritoProducto() { CarritoId = carritoId, ProductoId = productoId };
        var Producto = new Producto() { ProductoId = productoId, };
        var cliente = new ClienteDto() { ClienteId = clienteId };
        var carrito = new Carrito { CarritoId = Guid.NewGuid(), ClienteId = clienteId };
        _mockRepository.Setup(repo => repo.GetCarritoByClientId(clienteId))
            .ReturnsAsync(carrito);
        _mockRepository.Setup(repo => repo.CreateCarrito(carrito))
          .ReturnsAsync(carrito);
        _clienteService.Setup(clienteService => clienteService.GetClienteById(clienteId))
         .ReturnsAsync(cliente);
        _productService.Setup(productService => productService.FindRawProductById(clienteId))
        .ReturnsAsync(Producto);

        // Act
        var result = await _carritoService.AddCarritoProducto(compraCarritoDto);

        // Assert
        Assert.That(result is not null);
        Assert.That(carrito == result);
        _mockRepository.Verify(repo => repo.UpdateCarrito(carrito), Times.Once);
    }

    /// <summary>
    /// Tests that addCarritoProducto throws ArgumentException.
    /// </summary>
    [Test]
    public async Task AddCarritoProducto_ShouldThrowArgumentException()
    {
        int clienteId = 1;
        int productoId = 1;
        var carritoId = Guid.NewGuid();
        var cliente = new ClienteDto() { ClienteId = clienteId };
        var compraCarritoDto = new CompraCarritoDto() { ClienteId = clienteId, Cantidad = 0 };
        var carritoProducto = new CarritoProducto() { CarritoId = carritoId, ProductoId = productoId };
        var Producto = new Producto() { ProductoId = productoId, };

        var carrito = new Carrito { CarritoId = Guid.NewGuid(), ClienteId = clienteId };
        _mockRepository.Setup(repo => repo.GetCarritoByClientId(clienteId))
            .ReturnsAsync(carrito);
        _mockRepository.Setup(repo => repo.CreateCarrito(carrito))
          .ReturnsAsync(carrito);
        _clienteService.Setup(clienteService => clienteService.GetClienteById(clienteId))
            .ReturnsAsync((ClienteDto)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
         await _carritoService.AddCarritoProducto(compraCarritoDto));

        Assert.That(ex.Message, Is.EqualTo($"El Id del cliente ingresado no existe."));
    }


    /// <summary>
    /// Tests that DeleteProductoFromCarrito returns carrito.
    /// </summary>
    [Test]
    public async Task DeleteProductoFromCarrito_ShouldReturnCarrito()
    {
        // Arrange
        int clienteId = 1;
        int productoId = 1;
        var carritoId = Guid.NewGuid();

        var cliente = new ClienteDto() { ClienteId = clienteId };
        var compraCarritoDto = new CompraCarritoDto() { ClienteId = clienteId, Cantidad = 0 };
        var carritoProducto = new CarritoProducto() { CarritoId = carritoId, ProductoId = productoId };
        var Producto = new ProductoFindDto() { ProductoId = productoId, };
        var carritoProductoList = new List<CarritoProducto>
        {
            carritoProducto
        };
       var carrito = new Carrito { CarritoId = Guid.NewGuid(), CarritoProductos = carritoProductoList };

        _mockRepository.Setup(repo => repo.GetCarritoProductoById(It.IsAny<Guid>(), It.IsAny<int>()))
            .ReturnsAsync(carritoProducto);
        _mockRepository.Setup(repo => repo.GetCarritoByClientId(It.IsAny<int>()))
         .ReturnsAsync(carrito);
        _mockRepository.Setup(repo => repo.DeleteCarritoProducto(carritoProducto))
            .ReturnsAsync(carritoProducto);
        _clienteService.Setup(clienteService => clienteService.GetClienteById(clienteId))
          .ReturnsAsync(cliente);
        _productService.Setup(productService => productService.FindProductById(productoId))
          .ReturnsAsync(Producto);

        // Act
        var result = await _carritoService.DeleteProductoFromCarrito(clienteId,productoId);

        // Assert
        Assert.That(result is not null);
        Assert.That(carrito == result);
        _mockRepository.Verify(repo => repo.DeleteCarritoProducto(It.IsAny<CarritoProducto>()), Times.Once);
    }

    /// <summary>
    /// Tests that DeleteProductoFromCarrito throws ArgumentException
    /// </summary>
    [Test]
    public async Task DeleteProductoFromCarrito_ShouldThrowArgumentException()
    {
        // Arrange
        var carrito = new Carrito { CarritoId = Guid.NewGuid() };
        int clienteId = 1;
        int productoId = 1;
        var carritoId = Guid.NewGuid();

        _mockRepository.Setup(repo => repo.UpdateCarrito(carrito))
            .ReturnsAsync(carrito);
        _clienteService.Setup(clienteService => clienteService.GetClienteById(clienteId))
          .ReturnsAsync((ClienteDto)null);

        //Act & Assert

        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
         await _carritoService.DeleteProductoFromCarrito(clienteId, productoId));

        Assert.That(ex.Message, Is.EqualTo($"El Id del cliente ingresado no existe."));
    }

}
