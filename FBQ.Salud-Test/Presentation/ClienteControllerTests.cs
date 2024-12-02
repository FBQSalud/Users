using Application.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Presentation.Controllers;

namespace Tests.Presentation
{
    /// <summary>
    /// Unit tests for ClienteController.
    /// </summary>
    [TestFixture]
    public class ClienteControllerTests
    {
        private ClienteController _clienteController;
        private Mock<IClienteService> _clienteService;
        private int _clienteId;

        [SetUp]
        public void SetUp()
        {
            _clienteService = new Mock<IClienteService>();
            _clienteController = new ClienteController(_clienteService.Object);
            _clienteId = 1;
        }

        [Test]
        public async Task GetClienteById_ShouldReturnOk_WhenClienteIsFound()
        {
            // Arrange

            var clienteDto = new ClienteDto { ClienteId = _clienteId, Nombre = "Luis" };

            _clienteService
                .Setup(service => service.GetClienteById(_clienteId))
                .ReturnsAsync(clienteDto);

            // Act
            var result = await _clienteController.GetClienteById(_clienteId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(clienteDto));
        }

        [Test]
        public async Task GetClienteById_ShouldReturnNotFound_WhenClienteIsNotFound()
        {
            // Arrange
            _clienteService
                .Setup(service => service.GetClienteById(_clienteId))
                .ReturnsAsync((ClienteDto)null);

            // Act
            var result = await _clienteController.GetClienteById(_clienteId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetClienteById_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _clienteService
                .Setup(service => service.GetClienteById(_clienteId))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _clienteController.GetClienteById(_clienteId);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(objectResult.Value, Is.EqualTo("Internal Server Error"));
        }

        [Test]
        public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginRequest = new LoginRequestDto { Email = "test@example.com", Password = "password" };
            var cliente = new ClienteDto { ClienteId = 1, Nombre = "Luis" };

            _clienteService
                .Setup(service => service.GetClienteByEmailAndPasword(loginRequest.Email, loginRequest.Password))
                .ReturnsAsync(cliente);

            // Act
            var result = await _clienteController.Login(loginRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var loginInfoDto = okResult.Value as LoginInfoDto;
            Assert.That(loginInfoDto.ClienteId, Is.EqualTo(cliente.ClienteId));
            Assert.That(loginInfoDto.Nombre, Is.EqualTo(cliente.Nombre));
        }

        [Test]
        public async Task Login_ShouldReturnBadRequest_WhenEmailOrPasswordIsEmpty()
        {
            // Arrange
            var loginRequest = new LoginRequestDto { Email = "", Password = "" };

            // Act
            var result = await _clienteController.Login(loginRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo("Email and Password are required."));
        }

        [Test]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentials()
        {
            // Arrange
            var loginRequest = new LoginRequestDto { Email = "invalid@example.com", Password = "wrongpassword" };
            _clienteService
                .Setup(service => service.GetClienteByEmailAndPasword(loginRequest.Email, loginRequest.Password))
                .ReturnsAsync((ClienteDto)null);

            // Act
            var result = await _clienteController.Login(loginRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.That(unauthorizedResult.Value, Is.EqualTo("Invalid email or password."));
        }

        [Test]
        public async Task AddCliente_ShouldReturnCreated_WhenClienteIsAddedSuccessfully()
        {
            // Arrange
            var clienteDto = new ClienteDto { ClienteId = _clienteId, Nombre = "Luis" };
            _clienteService
                .Setup(service => service.CreateClient(clienteDto))
                .ReturnsAsync(clienteDto);

            // Act
            var result = await _clienteController.AddCliente(clienteDto);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedResult>());
            var createdResult = result as CreatedResult;
            Assert.That(createdResult.Value, Is.EqualTo(clienteDto));
        }

        [Test]
        public async Task AddCliente_ShouldReturnConflict_WhenDniAlreadyExists()
        {
            // Arrange
            var clienteDto = new ClienteDto { ClienteId = _clienteId, Nombre = "Luis" };
            _clienteService
                .Setup(service => service.CreateClient(clienteDto))
                .ReturnsAsync((ClienteDto)null);

            // Act
            var result = await _clienteController.AddCliente(clienteDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
            var conflictResult = result as ConflictObjectResult;
            var errorDto = conflictResult.Value as ErrorDto;
            Assert.That(errorDto.statuscode, Is.EqualTo("409"));
            Assert.That(errorDto.message, Is.EqualTo("Ha ocurrido un error. El DNI ingresado corresponde a un usuario ya existente en el sistema."));
        }

        [Test]
        public async Task AddCliente_ShouldReturnBadRequest_WhenInvalidDataFormat()
        {
            // Arrange
            var clienteDto = new ClienteDto(); // Assume empty data or invalid format
            _clienteService
                .Setup(service => service.CreateClient(clienteDto))
                .ThrowsAsync(new Exception("Invalid data format"));

            // Act
            var result = await _clienteController.AddCliente(clienteDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo("se ha ingresado los datos en un formato incorrecto"));
        }
    }
}
