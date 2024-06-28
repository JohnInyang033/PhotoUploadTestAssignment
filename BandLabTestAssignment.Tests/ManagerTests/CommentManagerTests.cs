using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;
using BandLabTestAssignment.Managers;
using BandLabTestAssignment.Repository.Interfaces;
using BandLabTestAssignment.Tests.Helper;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace BandLabTestAssignment.Tests.ManagerTests
{
    public class CommentManagerTests
    {
        private readonly Mock<ICommentRepository> _commentRepository;

        private readonly Mock<IUserRepository> _userRepository;

        private readonly CommentManager _manager;
        public CommentManagerTests()
        {
            _commentRepository = new Mock<ICommentRepository>();
            _userRepository = new Mock<IUserRepository>();
            _commentRepository.Setup(x => x.SaveChangesAsync());
            _commentRepository.Setup(x => x.Add(It.IsAny<Comment>()));
            _commentRepository.Setup(x => x.Remove(It.IsAny<Comment>()));
            _manager = new CommentManager(_commentRepository.Object, _userRepository.Object);
        }

        [Fact]
        public async Task SaveComment_ValidUser_Success()
        {
            //Arrange 
            _userRepository.Setup(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>())).Returns(true);

            //Act
            var result = await _manager.SaveComment(new CommentCreationDto());

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeOfType<CommentDto>();
            result.Success.Should().Be(true);
            _userRepository.Verify(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            _commentRepository.Verify(x => x.Add(It.IsAny<Comment>()), Times.Once);
            _commentRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SaveComment_InvalidUser_ReturnsError()
        {
            //Arrange 
            _userRepository.Setup(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>())).Returns(false);

            //Act
            var result = await _manager.SaveComment(new CommentCreationDto());

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().Be(false);
            _userRepository.Verify(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            _commentRepository.Verify(x => x.Add(It.IsAny<Comment>()), Times.Never);
            _commentRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }


        [Fact]
        public async Task DeleteComment_WhenCalled_ReturnsSuccess()
        {
            //Arrange 
            _userRepository.Setup(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>())).Returns(true);

            _commentRepository.Setup(x => x.GetCommentWithUserAsync(It.IsAny<int>())).ReturnsAsync(TestHelper.GetCommentWithUser());

            //Act
            var result = await _manager.DeleteComment(new CommentDeletionDto { CreatorId = 1 });

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().Be(true);
            _userRepository.Verify(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            _commentRepository.Verify(x => x.Remove(It.IsAny<Comment>()), Times.Once);
            _commentRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteComment_InvalidUser_ReturnsError()
        {
            //Arrange 
            _userRepository.Setup(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>())).Returns(false);

            _commentRepository.Setup(x => x.GetCommentWithUserAsync(It.IsAny<int>())).ReturnsAsync(TestHelper.GetCommentWithUser());

            //Act
            var result = await _manager.DeleteComment(new CommentDeletionDto { CreatorId = 1 });

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().Be(false);
            _userRepository.Verify(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            _commentRepository.Verify(x => x.GetCommentWithUserAsync(It.IsAny<int>()), Times.Never);
            _commentRepository.Verify(x => x.Remove(It.IsAny<Comment>()), Times.Never);
            _commentRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteComment_CommentNotFound_ReturnsError()
        {
            //Arrange 
            _userRepository.Setup(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>())).Returns(true);

            _commentRepository.Setup(x => x.GetCommentWithUserAsync(It.IsAny<int>()));

            //Act
            var result = await _manager.DeleteComment(new CommentDeletionDto { CreatorId = 1 });

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().Be(false);
            _userRepository.Verify(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            _commentRepository.Verify(x => x.GetCommentWithUserAsync(It.IsAny<int>()), Times.Once);
            _commentRepository.Verify(x => x.Remove(It.IsAny<Comment>()), Times.Never);
            _commentRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteComment_AnotherUserComment_ReturnsError()
        {
            //Arrange 
            _userRepository.Setup(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>())).Returns(true);

            _commentRepository.Setup(x => x.GetCommentWithUserAsync(It.IsAny<int>())).ReturnsAsync(TestHelper.GetCommentWithUser());

            //Act
            var result = await _manager.DeleteComment(new CommentDeletionDto { CreatorId = 2 });

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().Be(false);
            _userRepository.Verify(x => x.DoesExist(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            _commentRepository.Verify(x => x.GetCommentWithUserAsync(It.IsAny<int>()), Times.Once);
            _commentRepository.Verify(x => x.Remove(It.IsAny<Comment>()), Times.Never);
            _commentRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
