using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;
using BandLabTestAssignment.Managers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BandLabTestAssignment.Functions
{
    public class UserFunctions
    {
        private readonly ILogger<UserFunctions> _logger;
        private readonly IUserManager _userManager;

        public UserFunctions(ILogger<UserFunctions> logger,
            IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [Function("CreateUserFunction")]
        public async Task<IActionResult> CreateUserFunction([HttpTrigger(AuthorizationLevel.Anonymous, "post",
            Route = "user")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(CreateUserFunction)} HTTP trigger function processed a request.");

            try
            {
                string body = await new StreamReader(req.Body, Encoding.UTF8).ReadToEndAsync();
                var user = JsonSerializer.Deserialize<UserCreationDto>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                if (!user.IsValid())
                    return new BadRequestObjectResult(new ApiResponse<UserDto> { Errors = new() { "Invalid request object" } });

                var result = await _userManager.SaveUser(user);

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
