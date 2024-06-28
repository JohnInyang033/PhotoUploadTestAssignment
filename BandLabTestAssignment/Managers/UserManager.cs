using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;
using BandLabTestAssignment.Managers.Interfaces;
using BandLabTestAssignment.Repository.Interfaces;

namespace BandLabTestAssignment.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<UserDto>> SaveUser(UserCreationDto userCreationDto)
        {
            var apiResponse = new ApiResponse<UserDto>();

            var user = new User
            {
                UserName = userCreationDto.UserName,
                CreatedAt = DateTime.UtcNow,
            };

            _userRepository.Add(user);

            await _userRepository.SaveChangesAsync();

            apiResponse.Data = new UserDto
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                UserName = user.UserName
            };
            return apiResponse;
        }
    }
}
