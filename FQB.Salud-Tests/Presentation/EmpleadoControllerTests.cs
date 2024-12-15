using AutoMapper;
using FBQ.Salud_AccessData.Controllers;
using FBQ.Salud_Application.Services;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBQ.Salud_Presentation.Tests
{
    /// <summary>
    /// Unit tests for EmpleadoController, testing each of the CRUD actions.
    /// </summary>
    [TestFixture]
    public class EmpleadoControllerTests
    {
        private Mock<IUserServices> _userServiceMock;
        private Mock<IRolService> _rolServiceMock;
        private EmpleadoController _empleadoController;

        [SetUp]
        public void SetUp()
        {
            _userServiceMock = new Mock<IUserServices>();
            _rolServiceMock = new Mock<IRolService>();
            _empleadoController = new EmpleadoController(_userServiceMock.Object, null, _rolServiceMock.Object);
        }

        /// <summary>
        /// Tests the GetAll method. It ensures that it returns OK when there are users.
        /// </summary>
        [Test]
        public async Task GetAll_ShouldReturnOk_WhenUsersExist()
        {
            // Arrange
            var users = new List<UserResponse>
            {
                new UserResponse { EmployeeId = 1, UserName = "User1" },
                new UserResponse { EmployeeId = 2, UserName = "User2" }
            };

            _userServiceMock.Setup(x => x.GetAll()).ReturnsAsync(users);

            // Act
            var result = await _empleadoController.GetAll();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(users));
        }

        /// <summary>
        /// Tests the GetAll method. It ensures that it returns NotFound when no users exist.
        /// </summary>
        [Test]
        public async Task GetAll_ShouldReturnNotFound_WhenNoUsersExist()
        {
            // Arrange
            var users = new List<UserResponse>();

            _userServiceMock.Setup(x => x.GetAll()).ReturnsAsync(users);

            // Act
            var result = await _empleadoController.GetAll();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests the GetById method. It ensures that it returns Ok when the user is found.
        /// </summary>
        [Test]
        public async Task GetById_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new UserResponse { EmployeeId = userId, UserName = "User1" };

            _userServiceMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(user);

            // Act
            var result = await _empleadoController.GetById(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(user));
        }

        /// <summary>
        /// Tests the GetById method. It ensures that it returns a NotFound when the user is not found.
        /// </summary>
        [Test]
        public async Task GetById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _userServiceMock.Setup(x => x.GetUserById(userId)).ReturnsAsync((UserResponse)null);

            // Act
            var result = await _empleadoController.GetById(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<Response>());
            var response = result as Response;
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo($"empleados con id {userId} inexistente"));
        }


        /// <summary>
        /// Tests the CreateUser method. It ensures that it returns a Created status when the user is successfully created.
        /// </summary>
        [Test]
        public async Task CreateUser_ShouldReturnCreated_WhenUserIsCreated()
        {
            // Arrange
            var userRequest = new UserRequest { UserName = "NewUser", Email = "newuser@example.com" };
            var userResponse = new Response
            {
                Success = true,
                Message = "Exito",
                Result = userRequest
            };

            _userServiceMock.Setup(x => x.CreateUser(userRequest)).ReturnsAsync(userResponse);

            // Act
            var result = await _empleadoController.CreateUser(userRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<JsonResult>());
            var jsonResult = result as JsonResult;
            Assert.That(jsonResult.StatusCode, Is.EqualTo(201));
            Assert.That(jsonResult.Value, Is.EqualTo(userResponse));
        }

        /// <summary>
        /// Tests the CreateUser method. It ensures that it returns a Conflict status when there is an issue creating the user.
        /// </summary>
        [Test]
        public async Task CreateUser_ShouldReturnConflict_WhenUserCreationFails()
        {
            // Arrange
            var userRequest = new UserRequest { UserName = "NewUser", Email = "newuser@example.com" };
            var userResponse = new Response
            {
                Success = false,
                Message = "Conflict",
                Result = ""
            };

            _userServiceMock.Setup(x => x.CreateUser(userRequest)).ReturnsAsync(userResponse);

            // Act
            var result = await _empleadoController.CreateUser(userRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<JsonResult>());
            var jsonResult = result as JsonResult;
            Assert.That(jsonResult.StatusCode, Is.EqualTo(409));
            Assert.That(jsonResult.Value, Is.EqualTo(userResponse));
        }

        /// <summary>
        /// Tests the UpdateUser method. It ensures that it returns Ok when the user is updated.
        /// </summary>
        [Test]
        public async Task UpdateUser_ShouldReturnOk_WhenUserIsUpdated()
        {
            // Arrange
            var userId = 1;
            var userPut = new UserPut { UserName = "UpdatedUser" };
            var response = new Response
            {
                Success = true,
                Message = "Empleado modificado",
                Result = userPut
            };

            _userServiceMock.Setup(x => x.Update(userId, userPut)).ReturnsAsync(response);

            // Act
            var result = await _empleadoController.UpdateUser(userId, userPut);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        /// <summary>
        /// Tests the UpdateUser method. It ensures that it returns NotFound when the user cannot be updated.
        /// </summary>
        [Test]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserCannotBeUpdated()
        {
            // Arrange
            var userId = 1;
            var userPut = new UserPut { UserName = "UpdatedUser" };
            var response = new Response
            {
                Success = false,
                Message = "Empleado con id 1 inexistente",
                Result = ""
            };

            _userServiceMock.Setup(x => x.Update(userId, userPut)).ReturnsAsync(response);

            // Act
            var result = await _empleadoController.UpdateUser(userId, userPut);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        /// <summary>
        /// Tests the DeleteCliente method. It ensures that it returns Ok when the user is deleted.
        /// </summary>
        [Test]
        public async Task DeleteCliente_ShouldReturnOk_WhenUserIsDeleted()
        {
            // Arrange
            var userId = 1;
            var response = new Response
            {
                Success = true,
                Message = "Usuario eliminado",
                Result = ""
            };

            _userServiceMock.Setup(x => x.Delete(userId)).ReturnsAsync(response);

            // Act
            var result = await _empleadoController.DeleteCliente(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        /// <summary>
        /// Tests the DeleteCliente method. It ensures that it returns NotFound when the user cannot be deleted.
        /// </summary>
        [Test]
        public async Task DeleteCliente_ShouldReturnNotFound_WhenUserCannotBeDeleted()
        {
            // Arrange
            var userId = 1;
            var response = new Response
            {
                Success = false,
                Message = "Usuario inexistente",
                Result = ""
            };

            _userServiceMock.Setup(x => x.Delete(userId)).ReturnsAsync(response);

            // Act
            var result = await _empleadoController.DeleteCliente(userId);

            // Assert
            Assert.That(result, Is.InstanceOf<JsonResult>());
            var jsonResult = result as JsonResult;
            Assert.That(jsonResult.StatusCode, Is.EqualTo(404));
            Assert.That(jsonResult.Value, Is.EqualTo(response));
        }
    }
}
