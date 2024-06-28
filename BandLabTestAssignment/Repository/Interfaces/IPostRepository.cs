using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Common.Models.Response;

namespace BandLabTestAssignment.Repository.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<PostApiResponse<List<PostDto>>> GetPosts(int nextCursor, int pageSize);
    }
}
