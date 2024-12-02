using Application.DataAccess;
using Application.Services;
using Domain.Entities;
using Domain.Models;
using Moq;
using NUnit.Framework;

namespace Tests.Application;

    /// <summary>
    /// Unit tests for the ClienteService class.
    /// </summary>
    [TestFixture]
    public class ClienteServiceTests
    {
        private ClienteService _clienteService;
        private Mock<IClienteRepository> _mockClienteRepository;

        private int _clienteId;
        private Cliente _cliente;
        private ClienteDto _clienteDto;

        /// <summary>
        /// Set up the mock dependencies and data for testing.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockClienteRepository = new Mock<IClienteRepository>();
            _clienteService = new ClienteService(_mockClienteRepository.Object);

            _clienteId = 1;
            _cliente = new Cliente
            {
                ClienteId = _clienteId,
                Nombre = "Luis",
                Apellido = "Fernandez",
                Direccion = "123 Main St",
                Dni = "12345678",
                Mail = "luis.fernandez@example.com",
                Password = "password123",
                Telefono = "1234567890"
            };

            _clienteDto = new ClienteDto
            {
                ClienteId = _clienteId,
                Nombre = "Luis",
                Apellido = "Fernandez",
                Direccion = "123 Main St",
                Dni = "12345678",
                Mail = "luis.fernandez@example.com",
                Password = "password123",
                Telefono = "1234567890"
            };
        }

        /// <summary>
        /// Tests the GetClienteById method. It ensures that when a valid client ID is provided,
        /// the corresponding ClienteDto is returned.
        /// </summary>
        [Test]
        public async Task GetClienteById_ShouldReturnClienteDto_WhenClienteExists()
        {
            // Arrange
            _mockClienteRepository.Setup(repo => repo.GetClienteById(_clienteId)).ReturnsAsync(_cliente);

            // Act
            var result = await _clienteService.GetClienteById(_clienteId);

            // Assert
            Assert.That(result, Is.InstanceOf<ClienteDto>());
            Assert.That(result.Nombre, Is.EqualTo(_cliente.Nombre));
            Assert.That(result.Apellido, Is.EqualTo(_cliente.Apellido));
        }

        /// <summary>
        /// Tests the CreateClient method. It checks that when a client with the same DNI already exists,
        /// the method returns null.
        /// </summary>
        [Test]
        public async Task CreateClient_ShouldReturnNull_WhenClientWithDniExists()
        {
            // Arrange
            _mockClienteRepository.Setup(repo => repo.GetClienteByDNI(_clienteDto.Dni)).ReturnsAsync(_cliente);

            // Act
            var result = await _clienteService.CreateClient(_clienteDto);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests the CreateClient method. It ensures that when a client does not exist,
        /// the method returns the successfully created ClienteDto.
        /// </summary>
        [Test]
        public async Task CreateClient_ShouldReturnClienteDto_WhenClientIsCreatedSuccessfully()
        {
            // Arrange
            _mockClienteRepository.Setup(repo => repo.GetClienteByDNI(_clienteDto.Dni)).ReturnsAsync((Cliente)null);
            _mockClienteRepository.Setup(repo => repo.AddCliente(It.IsAny<Cliente>())).ReturnsAsync(_cliente);

            // Act
            var result = await _clienteService.CreateClient(_clienteDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ClienteDto>());
            Assert.That(result.Nombre, Is.EqualTo(_clienteDto.Nombre));
            Assert.That(result.Apellido, Is.EqualTo(_clienteDto.Apellido));
        }

        /// <summary>
        /// Tests the GetClienteByEmailAndPassword method. It verifies that when valid email and password
        /// are provided, the corresponding ClienteDto is returned.
        /// </summary>
        [Test]
        public async Task GetClienteByEmailAndPasword_ShouldReturnClienteDto_WhenValidCredentials()
        {
            // Arrange
            _mockClienteRepository.Setup(repo => repo.GetClienteByEmailAndPassword(_clienteDto.Mail, _clienteDto.Password)).ReturnsAsync(_cliente);

            // Act
            var result = await _clienteService.GetClienteByEmailAndPasword(_clienteDto.Mail, _clienteDto.Password);

            // Assert
            Assert.That(result, Is.InstanceOf<ClienteDto>());
            Assert.That(result.Nombre, Is.EqualTo(_cliente.Nombre));
            Assert.That(result.Apellido, Is.EqualTo(_cliente.Apellido));
        }

        /// <summary>
        /// Tests the GetClienteByEmailAndPassword method. It checks that when invalid credentials are provided,
        /// an exception is thrown with the message "Datos invalidos".
        /// </summary>
        [Test]
        public void GetClienteByEmailAndPasword_ShouldThrowException_WhenInvalidCredentials()
        {
            // Arrange
            _mockClienteRepository.Setup(repo => repo.GetClienteByEmailAndPassword(_clienteDto.Mail, _clienteDto.Password)).ReturnsAsync((Cliente)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _clienteService.GetClienteByEmailAndPasword(_clienteDto.Mail, _clienteDto.Password)
            );
            Assert.That(ex.Message, Is.EqualTo("Datos invalidos"));
        }
    }

