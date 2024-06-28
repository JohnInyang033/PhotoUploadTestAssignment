using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;

namespace BandLabTestAssignment.Managers.Interfaces
{
    public interface ICommentManager
    {
        Task<ApiResponse<bool>> DeleteComment(CommentDeletionDto commentDeletionDto);

        Task<ApiResponse<CommentDto>> SaveComment(CommentCreationDto commentCreationDto);
    }
}
