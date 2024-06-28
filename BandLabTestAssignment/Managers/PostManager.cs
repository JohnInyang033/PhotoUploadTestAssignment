using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;
using BandLabTestAssignment.Managers.Interfaces;
using BandLabTestAssignment.Repository.Interfaces;
using BandLabTestAssignment.Services.Interfaces;

namespace BandLabTestAssignment.Managers
{
    public class PostManager : IPostManager
    {
        private readonly IBlobService _blobService;

        private readonly IPostRepository _postRepository;

        private readonly IUserRepository _userRepository;

        public PostManager(IBlobService blobService,
            IPostRepository postRepository,
            IUserRepository userRepository)
        {
            _blobService = blobService;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task<PostApiResponse<List<PostDto>>> GetPosts(int nextCursor, int pageSize)
        {
            var posts = await _postRepository.GetPosts(nextCursor, pageSize);
            return posts;
        }

        public async Task<ApiResponse<bool>> SavePost(PostCreationDto postCreationDto, MemoryStream file, string fileName)
        {
            var apiResponse = new ApiResponse<bool>();

            if (!_userRepository.DoesExist(x => x.Id == postCreationDto.Creator))
            {
                apiResponse.Errors.Add("User does not exist");
                apiResponse.Success = false;
                return apiResponse;
            }

            var imageLink = await _blobService.UploadBlobFileAsync(file, fileName);

            var post = new Post
            {
                Caption = postCreationDto.Caption,
                CreatorId = postCreationDto.Creator,
                CreatedAt = DateTime.UtcNow,
                Image = imageLink
            };

            _postRepository.Add(post);
            await _postRepository.SaveChangesAsync();

            apiResponse.Data = true;

            return apiResponse;
        }
    }
}
