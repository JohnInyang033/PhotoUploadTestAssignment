using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;
using BandLabTestAssignment.Managers;
using BandLabTestAssignment.Repository.Interfaces;
using FluentAssertions;
using Moq;

namespace BandLabTestAssignment.Tests.ManagerTests
{
    public class UserManagerTests
    {
        private Mock<IUserRepository> _userRepository;

        private UserManager _manager;
        public UserManagerTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _userRepository.Setup(x => x.SaveChangesAsync());
            _userRepository.Setup(x => x.Add(It.IsAny<User>()));
            _manager = new UserManager(_userRepository.Object);
        }

        [Fact]
        public async Task SaveUser_ValidUser_Success()
        {
            //Arrange 
            _userRepository.Setup(x => x.SaveChangesAsync());
            _userRepository.Setup(x => x.Add(It.IsAny<User>()));

            //Act
            var result = await _manager.SaveUser(new UserCreationDto());

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeOfType<UserDto>();
            result.Success.Should().Be(true);
            _userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            _userRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
