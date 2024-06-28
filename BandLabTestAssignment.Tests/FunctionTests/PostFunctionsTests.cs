using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;
using BandLabTestAssignment.Functions;
using BandLabTestAssignment.Managers.Interfaces;
using BandLabTestAssignment.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;

namespace BandLabTestAssignment.Tests.FunctionTests
{
    public class PostFunctionsTests
    {
        private readonly Mock<IPostManager> _postManager;

        private readonly Mock<ILogger<PostFunctions>> _logger;

        private readonly IConfiguration _config;

        private readonly Mock<IImageService> _imageService;

        private PostFunctions _postFunctions;

        public PostFunctionsTests()
        {
            _postManager = new Mock<IPostManager>();
            _logger = new Mock<ILogger<PostFunctions>>();
            _imageService = new Mock<IImageService>();
            _config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                {"MaxFileSizeInMB", "100" }
            }).Build();

            _postFunctions = new PostFunctions(_postManager.Object, _logger.Object, _config, _imageService.Object);
        }

        [Fact]
        public async Task CreatePostFunction_WhenFileIsUploaded_ReturnsOk()
        {
            // Arrange
            _imageService.Setup(x => x.ResizeImage(It.IsAny<IFormFile>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new MemoryStream());
            _postManager.Setup(x => x.SavePost(It.IsAny<PostCreationDto>(), It.IsAny<MemoryStream>(), It.IsAny<string>())).ReturnsAsync(new ApiResponse<bool>());

            var formFileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Test file content");
            writer.Flush();
            ms.Position = 0;
            formFileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Callback<Stream, CancellationToken>((stream, _) =>
            {
                ms.CopyTo(stream);
            }).Returns(Task.CompletedTask);
            formFileMock.Setup(f => f.Length).Returns(ms.Length);
            formFileMock.Setup(f => f.FileName).Returns("test.jpg");
            formFileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            formFileMock.Setup(f => f.Name).Returns("file");

            var formCollection = new FormCollection(new Dictionary<string, StringValues>
            {
                {"Caption","New Post" },
                {"Creator","1" }
            }, new FormFileCollection { formFileMock.Object });

            var mockHttpRequest = new DefaultHttpContext().Request;
            mockHttpRequest.ContentType = "multipart/form-data";
            mockHttpRequest.Form = formCollection;

            // Act
            var result = await _postFunctions.CreatePostFunction(mockHttpRequest) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            var response = result.Value as ApiResponse<bool>;
            response.Success.Should().BeTrue();
        }

        [Fact]
        public async Task GetPostsFunction_WhenCalled_ReturnsOk()
        {
            // Arrange 
            _postManager.Setup(x => x.GetPosts(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new PostApiResponse<List<PostDto>>());

            var queryParameters = new Dictionary<string, StringValues>
            {
                {"cursor" ,"1"},
                {"pageSize" ,"100"},
            };
           
            var mockHttpRequest = new DefaultHttpContext().Request;
            mockHttpRequest.Query = new QueryCollection(queryParameters);

            // Act
            var result = await _postFunctions.GetPostsFunction(mockHttpRequest) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            var response = result.Value as PostApiResponse<List<PostDto>>;
            response.Success.Should().BeTrue();
        }
    }
}
