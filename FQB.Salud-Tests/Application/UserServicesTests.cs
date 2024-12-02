using AutoMapper;
using FBQ.Salud_Application.Services;
using FBQ.Salud_Application.Validations;
using FBQ.Salud_Domain.Commands;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using FBQ.Salud_Domain.Queries;
using Moq;

using NUnit.Framework;

/// <summary>
/// Unit tests for UsersServices, specifically testing CreateUser method.
/// </summary>
[TestFixture]
public class UsersServicesTests
{
    private Mock<IUserCommand> _userCommandMock;
    private Mock<IUserQuery> _userQueryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IUserValidatorExist> _userValidatorMock;
    private UsersServices _usersServices;

    [SetUp]
    public void SetUp()
    {
        _userCommandMock = new Mock<IUserCommand>();
        _userQueryMock = new Mock<IUserQuery>();
        _mapperMock = new Mock<IMapper>();
        _userValidatorMock = new Mock<IUserValidatorExist>();

        _usersServices = new UsersServices(
            _userCommandMock.Object,
            _userQueryMock.Object,
            _mapperMock.Object,
            _userValidatorMock.Object
        );
    }

    /// <summary>
    /// Tests the CreateUser method. It ensures that a user is created if the user does not exist or is reactivated if the user is soft-deleted.
    /// </summary>
    [Test]
    public async Task CreateUser_ShouldReturnSuccess_WhenUserIsCreatedOrReactivated()
    {
        // Arrange
        var userRequest = new UserRequest
        {
            DNI = "123456789",
            Email = "test@example.com",
            UserName = "Test User"
        };

        var userMapped = new User
        {
            DNI = "123456789",
            Email = "test@example.com",
            UserName = "Test User"
        };

        var existingUser = new User
        {
            DNI = "123456789",
            Email = "test@example.com",
            SoftDelete = true 
        };

        _userValidatorMock.Setup(v => v.ExisteUserAsync(It.IsAny<User>())).ReturnsAsync(false); 
        _userValidatorMock.Setup(v => v.ExisteEmailAsync(It.IsAny<User>())).ReturnsAsync(false); 

        _userQueryMock.Setup(q => q.GetUserByDNIAsync(It.IsAny<String>())).ReturnsAsync(existingUser);

 
        _mapperMock.Setup(m => m.Map<User>(It.IsAny<UserRequest>())).Returns(userMapped);
        _mapperMock.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse { UserName = "Test User" });

        // Act
        var result = await _usersServices.CreateUser(userRequest);

        // Assert
        Assert.That(result.Success, Is.True); 
        Assert.That(result.Message, Is.EqualTo("Empleado con dni 123456789 activado")); 
        Assert.That(result.Result, Is.Not.Null);
    }
}
