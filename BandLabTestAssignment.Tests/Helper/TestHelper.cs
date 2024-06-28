using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Common.Models.Response;

namespace BandLabTestAssignment.Tests.Helper
{
    public static class TestHelper
    {
        internal static Comment GetCommentWithUser()
        {
            return new Comment
            {
                Id = 1,
                Content = "Test Content",
                Creator = new User
                {
                    Id = 1
                }
            };
        }

        internal static PostApiResponse<List<PostDto>> GetPosts()
        {
            return new PostApiResponse<List<PostDto>>()
            {
                Data = new List<PostDto>
                {
                     new PostDto
                     {
                          Id = 1,
                           Caption = "Test Caption",
                            CreatedAt = DateTime.Now,
                             Image = "Test Image",
                            Comments = new List<CommentDto>
                            {
                                new CommentDto
                                {
                                     Id =  1,
                                      Content = "Test Content",
                                       CreatedAt = DateTime.Now
                                }
                            }
                     }
                }
            }; 
        }
    }
}
