using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Managers.Interfaces;
using BandLabTestAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BandLabTestAssignment.Functions
{
    public class PostFunctions
    {
        private readonly IPostManager _postManager;

        private readonly ILogger<PostFunctions> _logger;

        private readonly IConfiguration _config;

        private readonly IImageService _imageService;

        public PostFunctions(IPostManager postManager,
            ILogger<PostFunctions> logger,
            IConfiguration config,
            IImageService imageService)
        {
            _postManager = postManager;
            _logger = logger;
            _config = config;
            _imageService = imageService;
        }

        [Function("CreatePostFunction")]
        public async Task<IActionResult> CreatePostFunction([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "post")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation($"{nameof(CreatePostFunction)} HTTP trigger function processed a request.");

                var caption = req.Form[$"{nameof(PostCreationDto.Caption)}"];
                int.TryParse(req.Form[$"{nameof(PostCreationDto.Creator)}"], out int creatorId);

                var newPost = new PostCreationDto
                {
                    Caption = caption,
                    Creator = creatorId,
                };

                if (!newPost.IsValid())
                    return new BadRequestObjectResult(new ApiResponse<bool> { Errors = new() { "Invalid request object" } });

                var file = req.Form.Files[0];

                if (file == null)
                    return new BadRequestObjectResult(new ApiResponse<bool> { Errors = new() { "Invalid request object - Please upload a file" } });


                MemoryStream outputMemoryStream = null;

                // Check the file size (maximum 100MB)
                long maxFileSize = _config.GetValue<int>("MaxFileSizeInMB") * 1024 * 1024;
                if (file.Length > maxFileSize)
                    return new BadRequestObjectResult(new ApiResponse<bool> { Errors = new() { "File size exceeds the maximum allowed size of 100MB. Your file size is {file.Length / (1024 * 1024)}MB." } });


                // Check the file type 
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!Constants.AllowedExtensions.Contains(fileExtension))
                    return new BadRequestObjectResult(new ApiResponse<bool> { Errors = new() { "File format not allowed." } });

                outputMemoryStream = await _imageService.ResizeImage(file, Constants.ImageWidth, Constants.ImageHeight);

                //outputMemoryStream = resizedImage.ConvertToJPG(); 

                var result = await _postManager.SavePost(newPost, outputMemoryStream, file.FileName);

                if (result.Errors.Any())
                    return new BadRequestObjectResult(result);

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ObjectResult("Error occured while procesing request")
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

        }


        [Function("GetPostsFunction")]
        public async Task<IActionResult> GetPostsFunction([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "posts")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(GetPostsFunction)} HTTP trigger function processed a request.");
            try
            {
                req.Query.TryGetValue("cursor", out var cursor);

                req.Query.TryGetValue("pageSize", out var pageSize);

                int.TryParse(cursor, out var Cursor);

                int.TryParse(pageSize, out var PageSize);

                var result = await _postManager.GetPosts(Cursor, PageSize);

                if (result.Errors.Any())
                    return new BadRequestObjectResult(result);

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ObjectResult("Error occured while procesing request")
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
