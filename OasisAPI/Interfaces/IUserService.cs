using OasisAPI.Dto;
using OasisAPI.Models;

namespace OasisAPI.Interfaces;

public interface IUserService
{
    Task<OasisUserDto> CreateUser(OasisUser userData);
}