using Application.DataAccess;
using Application.Services;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Moq;
using NUnit.Framework;

namespace Tests.Application;

/// <summary>
/// Unit tests for OrdenService.
/// </summary>
[TestFixture]
public class OrdenServiceTests
{
    private OrdenService _ordenService;
    private Mock<IOrdenRepository> _ordenRepository;
    private Mock<ICarritoService> _carritoService;
    private Mock<IClienteService> _clienteService;
    private Mock<IMapper> _mapper;

    /// <summary>
    /// Set up the mock dependencies and data for testing the OrdenService.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _ordenRepository = new Mock<IOrdenRepository>();
        _carritoService = new Mock<ICarritoService>();
        _clienteService = new Mock<IClienteService>();
        _mapper = new Mock<IMapper>();

        _ordenService = new OrdenService(
            _ordenRepository.Object,
            _carritoService.Object,
            _clienteService.Object,
            _mapper.Object
        );
    }

    /// <summary>
    /// Tests the AddOrden method. Verifies that when valid client and carrito data are provided,
    /// a new order is created and returned.
    /// </summary>
    [Test]
    public async Task AddOrden_ValidClientAndCarrito_ShouldReturnNewOrden()
    {
        // Arrange
        int clientId = 1;
        var carritoid = Guid.NewGuid();
        var client = new ClienteDto { ClienteId = clientId, Nombre = "Test Client" };
        var orden = new Orden
        {
            OrdenId = carritoid,
            CarritoId = carritoid,
            Total = 1200,
            Fecha = DateTime.Now
        };
        var carrito = new Carrito
        {
            CarritoId = carritoid,
            ClienteId = clientId,
            Estado = true,
            CarritoProductos = new List<CarritoProducto>
            {
                new() {
                    Producto = new Producto { Precio = 100 },
                    Cantidad = 2
                }
            }
        };

        _clienteService.Setup(cliente => cliente.GetClienteById(clientId)).ReturnsAsync(client);
        _carritoService.Setup(c => c.GetCarritoByClientId(clientId)).ReturnsAsync(carrito);
        _ordenRepository.Setup(r => r.CreateOrden(It.IsAny<Orden>())).ReturnsAsync(orden);

        // Act
        var response = await _ordenService.AddOrden(clientId);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(response.Total, Is.EqualTo(200));
            Assert.That(response.CarritoId, Is.EqualTo(carrito.CarritoId));
        });
    }

    /// <summary>
    /// Tests the AddOrden method. Verifies that when the client does not exist,
    /// an ArgumentException is thrown with a specific message.
    /// </summary>
    [Test]
    public void AddOrden_ClientNotFound_ShouldThrowArgumentException()
    {
        // Arrange
        int clientId = 1;
        _clienteService.Setup(c => c.GetClienteById(clientId)).ReturnsAsync((ClienteDto)null);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(
            () => _ordenService.AddOrden(clientId),
            "El Id del cliente ingresado no existe."
        );
    }

    /// <summary>
    /// Tests the AddOrden method. Verifies that when the client's carrito is empty,
    /// an InvalidOperationException is thrown with a specific message.
    /// </summary>
    [Test]
    public void AddOrden_EmptyCarrito_ShouldThrowInvalidOperationException()
    {
        // Arrange
        int clientId = 1;
        var client = new ClienteDto { ClienteId = clientId, Nombre = "Test Client" };
        _clienteService.Setup(c => c.GetClienteById(clientId)).ReturnsAsync(client);
        _carritoService.Setup(c => c.GetCarritoByClientId(clientId))
            .ReturnsAsync(new Carrito { CarritoProductos = new List<CarritoProducto>() });

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(
            () => _ordenService.AddOrden(clientId),
            "El carrito está vacío, no contiene productos."
        );
    }

    /// <summary>
    /// Tests the GetOrder method. Verifies that when valid data is provided, 
    /// the corresponding order response is returned with details such as total pages.
    /// </summary>
    [Test]
    public async Task GetOrder_ValidData_ShouldReturnOrdenResponse()
    {
        // Arrange
        int limit = 5, page = 1;
        var orders = new List<Orden>
        {
            new Orden { OrdenId = Guid.NewGuid(), CarritoId = Guid.NewGuid(), Fecha = DateTime.Now }
        };

        _ordenRepository.Setup(r => r.GetAllOrders(null, null)).ReturnsAsync(orders);
        _ordenRepository.Setup(r => r.GetOrderByPage(limit, page, null, null)).ReturnsAsync(orders);

        var carrito = new Carrito
        {
            CarritoId = orders.First().CarritoId,
            ClienteId = 1,
            CarritoProductos = new List<CarritoProducto>
            {
                new CarritoProducto
                {
                    Producto = new Producto { Precio = 100 },
                    Cantidad = 2
                }
            }
        };
        var client = new ClienteDto { ClienteId = 1, Nombre = "Test Client" };
        var orderDto = new OrdenDto();

        _carritoService.Setup(c => c.GetRawCarritoById(It.IsAny<Guid>())).ReturnsAsync(carrito);
        _clienteService.Setup(c => c.GetClienteById(1)).ReturnsAsync(client);
        _mapper.Setup(m => m.Map<OrdenDto>(It.IsAny<Orden>())).Returns(orderDto);

        // Act
        var response = await _ordenService.GetOrder(limit, page);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.TotalPages, Is.EqualTo(1));
        Assert.That(response.Orders, Is.Not.Empty);
    }

    /// <summary>
    /// Tests the GetOrder method. Verifies that when no orders are found,
    /// the method returns null.
    /// </summary>
    [Test]
    public async Task GetOrder_NoOrdersFound_ShouldReturnNull()
    {
        // Arrange
        _ordenRepository.Setup(r => r.GetAllOrders(null, null)).ReturnsAsync(new List<Orden>());

        // Act
        var response = await _ordenService.GetOrder(5, 1);

        // Assert
        Assert.That(response, Is.Null);
    }

    /// <summary>
    /// Tests the GetOrder method. Verifies that when an exception occurs,
    /// an InvalidOperationException is thrown with a specific message.
    /// </summary>
    [Test]
    public void GetOrder_Exception_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _ordenRepository.Setup(r => r.GetAllOrders(null, null)).ThrowsAsync(new Exception("Unexpected error"));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(
            () => _ordenService.GetOrder(5, 1),
            "An error occurred while fetching orders."
        );
    }
}
