using OasisAPI.Dto;
using OasisAPI.Models;

namespace OasisAPI.Interfaces.Services;

public interface IUserService
{
    Task<OasisApiResult<OasisUserResponseDto>> CreateUserAsync(OasisUser userData);
}