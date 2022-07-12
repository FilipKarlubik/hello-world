﻿using Eucyon_Tribes.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Eucyon_Tribes.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Eucyon_Tribes.Factories;
using Microsoft.Extensions.Configuration;

namespace Tribes.Tests.UserTests
{
    [Serializable]
    [Collection("Serialize")]
    public class UserControllerTests : IDisposable
    {
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "UsersList2").Options;

        public ApplicationContext db;
        public UserRestController userRestController;
        public UserService userService;
        public KingdomService kingdomService;
        public KingdomFactory kingdomFactory;
        public BuildingFactory buildingFactory;
        public ResourceFactory resourceFactory;
        public IAuthService authService;

        public UserControllerTests()
        {
            db = new ApplicationContext(options);
            resourceFactory = new ResourceFactory();
            buildingFactory = new BuildingFactory();
            kingdomFactory = new KingdomFactory(db, resourceFactory, buildingFactory);
            kingdomService = new KingdomService(db, kingdomFactory);
            var config = new ConfigurationBuilder().AddUserSecrets("5ea770c2-4c16-4659-94eb-5a89323b961c").Build();
            authService = new JWTService(config);
            userService = new UserService(db, kingdomService, authService);
            userRestController = new UserRestController(userService);

            var user1 = new User()
            {
                Name = "Matilda",
                PasswordHash = "m",
                Email = "matilda@gmail.com",
                VerificationToken = "",
                ForgottenPasswordToken = ""
            };

            var user2 = new User()
            {
                Name = "Klotilda",
                PasswordHash = "k",
                Email = "klotilda@gmail.com",
                VerificationToken = "",
                ForgottenPasswordToken = ""
            };
            db.Worlds.Add(new World());
            db.Users.Add(user1);
            db.Users.Add(user2);
            db.SaveChanges();
        }

        public void Dispose()
        {
            foreach (var user in db.Users)
                db.Users.Remove(user);
            db.SaveChanges();
        }

        [Theory]
        [InlineData(200)]
        public void Show_User_with_valid_id(int statuscode)
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(i => i.ShowUser(It.IsAny<int>())).Returns(new UserResponseDto(1, "", DateTime.Now));
            var userRestController = new UserRestController(mockUserService.Object);
            int id = 1;
            // Act
            var result = (ObjectResult)userRestController.Show(id);
            // Assert
            Assert.Equal(statuscode, result.StatusCode);
        }

        [Theory]
        [InlineData(400)]
        public void Show_User_with_invalid_id(int statuscode)
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(i => i.ShowUser(It.IsAny<int>())).Returns(new UserResponseDto(1, "", DateTime.Now));
            var userRestController = new UserRestController(mockUserService.Object);
            int id = 0;
            // Act
            var result = (ObjectResult)userRestController.Show(id);
            // Assert
            Assert.Equal(statuscode, result.StatusCode);

        }

        [Theory]
        [InlineData(404)]
        public void Show_User_with_valid_non_existing_id(int statuscode)
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(i => i.ShowUser(It.IsAny<int>())).Returns<UserResponseDto>(null);
            var userRestController = new UserRestController(mockUserService.Object);
            // Act
            var result = (ObjectResult)userRestController.Index();
            // Assert
            Assert.Equal(statuscode, result.StatusCode);
        }

        [Theory]
        [InlineData(200)]
        public void List_Users_existing_in_database(int statuscode)
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(i => i.ListAllUsers()).Returns(new List<UserResponseDto>());
            var userRestController = new UserRestController(mockUserService.Object);
            // Act
            var result = (ObjectResult)userRestController.Index();
            // Assert
            Assert.Equal(statuscode, result.StatusCode);
        }

        [Theory]
        [InlineData(404)]
        public void List_Users_in_database_with_no_Users(int statuscode)
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(i => i.ListAllUsers()).Returns<UserResponseDto>(null);
            var userRestController = new UserRestController(mockUserService.Object);
            // Act
            var result = (ObjectResult)userRestController.Index();
            // Assert
            Assert.Equal(statuscode, result.StatusCode);
        }

        [Fact]
        public void Destroy_by_existing_id()
        {

            // Arrange
            var id = db.Users.FirstOrDefault(u => u.Name.Equals("Matilda")).Id;
            var pass = db.Users.FirstOrDefault(u => u.Name.Equals("Matilda")).PasswordHash;
            // Act
            var result = (ObjectResult)userRestController.Destroy(id, pass);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(1, db.Users.Count());
        }

        [Fact]
        public void Destroy_by_non_existing_id()
        {

            // Arrange
            var id = 15000;
            var pass = db.Users.FirstOrDefault(u => u.Name.Equals("Matilda")).PasswordHash;
            // Act
            var result = (ObjectResult)userRestController.Destroy(id, pass);

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal(2, db.Users.Count());
        }
    }
}
