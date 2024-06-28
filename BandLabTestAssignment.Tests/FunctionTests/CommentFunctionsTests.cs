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
    public class CommentFunctionsTests
    {
        private readonly Mock<ILogger<CommentFunctions>> _logger;

        private readonly Mock<ICommentManager> _commentManager;

        private readonly CommentFunctions _commentFunctions;
        public CommentFunctionsTests()
        {
            _logger = new Mock<ILogger<CommentFunctions>>();
            _commentManager = new Mock<ICommentManager>();

            _commentFunctions = new CommentFunctions(_logger.Object, _commentManager.Object);
        }

        [Fact]
        public async Task CreateCommentFunction_WhenCalled_ReturnsOk()
        {
            // Arrange 
            _commentManager.Setup(x => x.SaveComment(It.IsAny<CommentCreationDto>())).ReturnsAsync(new ApiResponse<CommentDto>());

            var mockHttpRequest = new DefaultHttpContext().Request;

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            var json = JsonConvert.SerializeObject(new CommentCreationDto
            {
                CreatorId = 1,
                PostId = 1,
                Content = "Test Content"
            });
            writer.Write(json);
            writer.Flush();
            ms.Position = 0;

            mockHttpRequest.Body = ms;

            // Act
            var result = await _commentFunctions.CreateCommentFunction(mockHttpRequest) as OkObjectResult;

            // Assert
            _commentManager.Verify(x => x.SaveComment(It.IsAny<CommentCreationDto>()), Times.Once);
            result.Should().NotBeNull();
            var response = result.Value as ApiResponse<CommentDto>;
            response.Success.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteCommentFunction_WhenCalled_ReturnsOk()
        {
            // Arrange 
            _commentManager.Setup(x => x.DeleteComment(It.IsAny<CommentDeletionDto>())).ReturnsAsync(new ApiResponse<bool>());

            var mockHttpRequest = new DefaultHttpContext().Request;

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            var json = JsonConvert.SerializeObject(new CommentDeletionDto
            {
                CreatorId = 1,
                Id = 1,
            });
            writer.Write(json);
            writer.Flush();
            ms.Position = 0;

            mockHttpRequest.Body = ms;

            // Act
            var result = await _commentFunctions.DeleteCommentFunction(mockHttpRequest) as OkObjectResult;

            // Assert
            _commentManager.Verify(x => x.DeleteComment(It.IsAny<CommentDeletionDto>()), Times.Once);
            result.Should().NotBeNull();
            var response = result.Value as ApiResponse<bool>;
            response.Success.Should().BeTrue();
        }
    }
}
