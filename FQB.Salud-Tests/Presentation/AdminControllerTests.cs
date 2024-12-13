using AutoMapper;
using FBQ.Salud_Application.Services;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FBQ.Salud_Presentation.Tests
{
    /// <summary>
    /// Unit tests for adminController, specifically testing the Login method.
    /// </summary>
    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<IRolService> _rolServiceMock;
        private Mock<IUserServices> _userServiceMock;
        private Mock<IMapper> _mapperMock;
        private adminController _adminController;

        [SetUp]
        public void SetUp()
        {
            // Crear mocks para los servicios necesarios
            _rolServiceMock = new Mock<IRolService>();
            _userServiceMock = new Mock<IUserServices>();
            _mapperMock = new Mock<IMapper>();

            // Pasar los mocks al constructor del controlador
            _adminController = new adminController(
                _rolServiceMock.Object,
                _userServiceMock.Object,
                _mapperMock.Object
            );
        }

        /// <summary>
        /// Tests the Login method. It ensures that the correct action is taken when valid credentials are provided.
        /// </summary>
        [Test]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var adminRequest = new AdminRequest
            {
                Email = "test@example.com",
                Password = "password"
            };

            var loginResponse = new Response
            {
                Success = true,
                Message = "exito",
                Result = "token"
            };

            _rolServiceMock.Setup(x => x.LoginUser(adminRequest.Email, adminRequest.Password))
                .ReturnsAsync(loginResponse);

            // Act
            var result = await _adminController.Login(adminRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(loginResponse));
        }

        /// <summary>
        /// Tests the Login method. It ensures that the correct action is taken when invalid credentials are provided.
        /// </summary>
        [Test]
        public async Task Login_ShouldReturnNotFound_WhenCredentialsAreInvalid()
        {
            // Arrange
            var adminRequest = new AdminRequest
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var loginResponse = new Response
            {
                Success = false,
                Message = "credenciales incorrectas",
                Result = ""
            };

            _rolServiceMock.Setup(x => x.LoginUser(adminRequest.Email, adminRequest.Password))
                .ReturnsAsync(loginResponse);

            // Act
            var result = await _adminController.Login(adminRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.Value, Is.EqualTo(loginResponse));
        }

        /// <summary>
        /// Tests the Login method. It ensures that the correct action is taken when an exception occurs during login.
        /// </summary>
        [Test]
        public async Task Login_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var adminRequest = new AdminRequest
            {
                Email = "test@example.com",
                Password = "password"
            };

            _rolServiceMock.Setup(x => x.LoginUser(adminRequest.Email, adminRequest.Password))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _adminController.Login(adminRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }
    }
}
