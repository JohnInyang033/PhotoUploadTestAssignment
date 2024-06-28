using BandLabTestAssignment.Common.Models.Request;
using BandLabTestAssignment.Common.Models.Response;

namespace BandLabTestAssignment.Managers.Interfaces
{
    public interface IUserManager
    {
        Task<ApiResponse<UserDto>> SaveUser(UserCreationDto userCreationDto);
    }
}
