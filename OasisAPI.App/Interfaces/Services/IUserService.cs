using Domain.Entities;
using OasisAPI.App.Dto;
using OasisAPI.App.Dto.Response;
using OasisAPI.App.Result;

namespace OasisAPI.Interfaces.Services;

public interface IUserService
{
    Task<AppResult<OasisUserResponseDto>> CreateUserAsync(OasisUser userData);
}