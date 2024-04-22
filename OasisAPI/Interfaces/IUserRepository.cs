using OasisAPI.Dto;
using OasisAPI.Models;

namespace OasisAPI.Interfaces;

public interface IUserRepository
{
    Task<OasisUser> CreateUserAsync(OasisUser userData);
    Task<OasisUser?> GetUserByEmail(string email);
    Task<OasisUser?> GetUserById(int id);
}