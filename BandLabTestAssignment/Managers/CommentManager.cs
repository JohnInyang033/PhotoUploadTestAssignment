using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;
using BandLabTestAssignment.Managers.Interfaces;
using BandLabTestAssignment.Repository.Interfaces;

namespace BandLabTestAssignment.Managers
{
    public class CommentManager : ICommentManager
    {
        private readonly ICommentRepository _commentRepository;

        private readonly IUserRepository _userRepository;

        public CommentManager(ICommentRepository commentRepository,
            IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<CommentDto>> SaveComment(CommentCreationDto commentCreationDto)
        {
            var apiResponse = new ApiResponse<CommentDto>();

            if (!_userRepository.DoesExist(x => x.Id == commentCreationDto.CreatorId))
            {
                apiResponse.Errors.Add("User does not exist");
                apiResponse.Success = false;
                return apiResponse;
            }

            var comment = new Comment
            {
                Content = commentCreationDto.Content,
                CreatorId = commentCreationDto.CreatorId,
                PostId = commentCreationDto.PostId,
                CreatedAt = DateTime.UtcNow
            };

            _commentRepository.Add(comment);
            await _commentRepository.SaveChangesAsync();

            apiResponse.Data = new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatorId = comment.CreatorId,
                CreatedAt = comment.CreatedAt
            };

            return apiResponse;
        }

        public async Task<ApiResponse<bool>> DeleteComment(CommentDeletionDto commentDeletionDto)
        {
            var apiResponse = new ApiResponse<bool>();

            if (!_userRepository.DoesExist(x => x.Id == commentDeletionDto.CreatorId))
            {
                apiResponse.Errors.Add("User does not exist");
                apiResponse.Success = false;
                return apiResponse;
            }

            var comment = await _commentRepository.GetCommentWithUserAsync(commentDeletionDto.Id);

            if (comment == null)
            {
                apiResponse.Errors.Add("Comment not found");
                apiResponse.Success = false;
                return apiResponse;
            }


            if (comment.Creator.Id != commentDeletionDto.CreatorId)
            {
                apiResponse.Errors.Add("You cannot delete other user's comment");
                apiResponse.Success = false;
                return apiResponse;
            }

            _commentRepository.Remove(comment);
            await _commentRepository.SaveChangesAsync();

            apiResponse.Data = true;
            return apiResponse;
        }
    }
}
