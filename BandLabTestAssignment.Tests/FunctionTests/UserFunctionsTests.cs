using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;
using BandLabTestAssignment.Functions;
using BandLabTestAssignment.Managers.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace BandLabTestAssignment.Tests.FunctionTests
{
    public class UserFunctionsTests
    {
        private readonly Mock<ILogger<UserFunctions>> _logger;

        private readonly Mock<IUserManager> _userManager;

        private readonly UserFunctions _userFunctions;
        public UserFunctionsTests()
        {
            _logger = new Mock<ILogger<UserFunctions>>();
            _userManager = new Mock<IUserManager>();

            _userFunctions = new UserFunctions(_logger.Object, _userManager.Object);
        }

        [Fact]
        public async Task CreateCommentFunction_WhenCalled_ReturnsOk()
        {
            // Arrange 
            _userManager.Setup(x => x.SaveUser(It.IsAny<UserCreationDto>())).ReturnsAsync(new ApiResponse<UserDto>());

            var mockHttpRequest = new DefaultHttpContext().Request;

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            var json = JsonConvert.SerializeObject(new UserCreationDto
            {
                UserName = "Test User",
            });
            writer.Write(json);
            writer.Flush();
            ms.Position = 0;

            mockHttpRequest.Body = ms;

            // Act
            var result = await _userFunctions.CreateUserFunction(mockHttpRequest) as OkObjectResult;

            // Assert
            _userManager.Verify(x => x.SaveUser(It.IsAny<UserCreationDto>()), Times.Once);
            result.Should().NotBeNull();
            var response = result.Value as ApiResponse<UserDto>;
            response.Success.Should().BeTrue();
        }
    }
}
