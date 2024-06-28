using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Common.Models.Response;
using BandLabTestAssignment.Data;
using BandLabTestAssignment.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BandLabTestAssignment.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PostApiResponse<List<PostDto>>> GetPosts(int nextCursor, int pageSize)
        {
            var apiResponse = new PostApiResponse<List<PostDto>>();

            var posts = await _context.Posts.AsNoTracking().Select(x => new PostDto
            {
                Id = x.Id,
                Image = x.Image,
                Caption = x.Caption,
                CreatedAt = x.CreatedAt,
                Comments = x.Comments.OrderByDescending(c => c.CreatedAt).Select(x => new CommentDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,
                    CreatorId = x.CreatorId
                }).Take(Constants.CommentCount).ToList(),
                CommentCount = x.Comments.Count
            })
            .Where(x => x.Id > nextCursor)
            .Take(pageSize + 1)
            .OrderByDescending(p => p.CommentCount)
            .ToListAsync();

            apiResponse.Data = posts;

            var cursor = posts.LastOrDefault()?.Id;
            apiResponse.Cursor = cursor == null ? 0 : cursor.Value;

            return apiResponse;
        }
    }
}
