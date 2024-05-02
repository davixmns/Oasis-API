using OasisAPI.Dto;
using OasisAPI.Models;

namespace OasisAPI.Interfaces.Services;

public interface IUserService
{
    Task<OasisApiResponse<OasisUserDto>> CreateUserAsync(OasisUser userData);
}