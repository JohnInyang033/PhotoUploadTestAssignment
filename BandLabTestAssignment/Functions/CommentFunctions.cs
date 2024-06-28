using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Managers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;
using System.Net;

namespace BandLabTestAssignment.Functions
{
    public class CommentFunctions
    {
        private readonly ILogger<CommentFunctions> _logger;
        private readonly ICommentManager _commentManager;

        public CommentFunctions(ILogger<CommentFunctions> logger,
            ICommentManager commentManager)
        {
            _logger = logger;
            _commentManager = commentManager;
        }

        [Function("CreateCommentFunction")]
        public async Task<IActionResult> CreateCommentFunction([HttpTrigger(AuthorizationLevel.Anonymous,"post",Route = "comment")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(CreateCommentFunction)} HTTP trigger function processed a request.");
            try
            {
                string body = await new StreamReader(req.Body, Encoding.UTF8).ReadToEndAsync();
                var comment = JsonSerializer.Deserialize<CommentCreationDto>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                if (!comment.IsValid())
                    return new BadRequestObjectResult(new ApiResponse<bool> { Errors = new() { "Invalid request object" } });

                var result = await _commentManager.SaveComment(comment);

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


        [Function("DeleteCommentFunction")]
        public async Task<IActionResult> DeleteCommentFunction([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "comment/delete")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(DeleteCommentFunction)} HTTP trigger function processed a request.");
            try
            {
                string body = await new StreamReader(req.Body, Encoding.UTF8).ReadToEndAsync();
                var comment = JsonSerializer.Deserialize<CommentDeletionDto>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                if (!comment.IsValid())
                    return new BadRequestObjectResult(new ApiResponse<bool> { Errors = new() { "Invalid request object" } });

                var result = await _commentManager.DeleteComment(comment);

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
