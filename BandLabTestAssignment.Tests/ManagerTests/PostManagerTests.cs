using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Managers;
using BandLabTestAssignment.Repository.Interfaces;
using BandLabTestAssignment.Services.Interfaces;
using BandLabTestAssignment.Tests.Helper;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace BandLabTestAssignment.Tests.ManagerTests
{
    public class PostManagerTests
    {

        private Mock<IBlobService> _blobService;

        private Mock<IPostRepository> _postRepository;

        private Mock<IUserRepository> _userRepository;

        private PostManager _manager;
        public PostManagerTests()
        {
            _blobService = new Mock<IBlobService>();
            _postRepository = new Mock<IPostRepository>();
            _postRepository.Setup(x => x.Add(It.IsAny<Post>())); 
            _postRepository.Setup(x => x.SaveChangesAsync());
            _userRepository = new Mock<IUserRepository>();
            _manager = new PostManager(_blobService.Object, _postRepository.Object, _userRepository.Object);
        }

        [Fact]
        public async Task GetPosts_ReturnsSuccess()
        {
            //Arrange
            _postRepository.Setup(x => x.GetPosts(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(TestHelper.GetPosts());

            //Act
            var result = await _manager.GetPosts(1, 100);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task SavePost_ValidUser_ReturnsSuccess()
        {
            //Arrange
            _blobService.Setup(x => x.UploadBlobFileAsync(It.IsAny<MemoryStream>(), It.IsAny<string>())).ReturnsAsync("image");

            _userRepository.Setup(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>())).Returns(true); 

            //Act
            var result = await _manager.SavePost(new PostCreationDto(), null, "");

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().Be(true);
            result.Success.Should().Be(true);
            _userRepository.Verify(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            _postRepository.Verify(x => x.Add(It.IsAny<Post>()), Times.Once);
            _postRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SavePost_InvalidUser_ReturnsError()
        {
            //Arrange
            _blobService.Setup(x => x.UploadBlobFileAsync(It.IsAny<MemoryStream>(), It.IsAny<string>())).ReturnsAsync("image");

            _userRepository.Setup(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>())).Returns(false); 

            //Act
            var result = await _manager.SavePost(new PostCreationDto(), null, "");

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().Be(false);
            _blobService.Verify(x => x.UploadBlobFileAsync(It.IsAny<MemoryStream>(), It.IsAny<string>()), Times.Never);

            _userRepository.Verify(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            _postRepository.Verify(x => x.Add(It.IsAny<Post>()), Times.Never);
            _postRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
