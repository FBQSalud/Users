using Moq;
using NUnit.Framework;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using FBQ.Salud_Application.Services;
using FBQ.Salud_Domain.Queries;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;

namespace FBQ.Salud_Application.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="RolServices"/> class.
    /// These tests verify the behavior of the LoginUser and ValidarToken methods.
    /// </summary>
    [TestFixture]
    public class RolServicesTests
    {
        private Mock<IRolQuery> _rolQueryMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IAdminQuery> _adminQueryMock;
        private RolServices _rolServices;

        /// <summary>
        /// Set up the necessary mock objects and the service instance before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _rolQueryMock = new Mock<IRolQuery>();
            _configurationMock = new Mock<IConfiguration>();
            _mapperMock = new Mock<IMapper>();
            _adminQueryMock = new Mock<IAdminQuery>();

            _rolServices = new RolServices(
                _rolQueryMock.Object,
                _configurationMock.Object,
                _mapperMock.Object,
                _adminQueryMock.Object
            );
        }

  
        /// <summary>
        /// Tests that LoginUser returns an error when invalid credentials are provided.
        /// </summary>
        [Test]
        public async Task LoginUser_ShouldReturnError_WhenCredentialsAreInvalid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrongpassword";
            _rolQueryMock.Setup(x => x.LoginUser(email, password)).ReturnsAsync((User)null);

            // Act
            var result = await _rolServices.LoginUser(email, password);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("credenciales incorrectas"));
        }

        /// <summary>
        /// Tests that ValidarToken returns an admin when a valid token is provided.
        /// </summary>
        [Test]
        public void ValidarToken_ShouldReturnAdmin_WhenTokenIsValid()
        {
            // Arrange
            var identity = new ClaimsIdentity();
            var claims = new[]
            {
                new Claim("id", "1"),
                new Claim(JwtRegisteredClaimNames.Sub, "TestSubject")
            };
            identity.AddClaims(claims);

            var admin = new User { UserId = 1, UserName = "AdminUser" };
            _adminQueryMock.Setup(x => x.GetAdminById("1")).Returns(admin);

            // Act
            var result = _rolServices.ValidarToken(identity);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Exito "));
            Assert.That(result.Result, Is.EqualTo(admin));
        }

        /// <summary>
        /// Tests that ValidarToken returns an error when no claims exist in the token.
        /// </summary>
        [Test]
        public void ValidarToken_ShouldReturnError_WhenNoClaimsExist()
        {
            // Arrange
            var identity = new ClaimsIdentity();

            // Act
            var result = _rolServices.ValidarToken(identity);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Verificar token valido"));
        }

        /// <summary>
        /// Tests that ValidarToken returns an error when an exception occurs during token validation.
        /// </summary>
        [Test]
        public void ValidarToken_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            var identity = new ClaimsIdentity();
            _adminQueryMock.Setup(x => x.GetAdminById(It.IsAny<string>())).Throws(new Exception("Some error"));

            // Act
            var result = _rolServices.ValidarToken(identity);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Verificar token valido"));
        }
    }
}
