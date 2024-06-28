using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;

namespace BandLabTestAssignment.Managers.Interfaces
{
    public interface IPostManager
    {
        Task<ApiResponse<bool>> SavePost(PostCreationDto postCreationDto, MemoryStream file, string fileName);

        Task<PostApiResponse<List<PostDto>>> GetPosts(int nextCursor,int pageSize); 
    }
}
