using OasisAPI.Dto;
using OasisAPI.Models;

namespace OasisAPI.Interfaces;

public interface IUserRepository : IRepository<OasisUser>
{
    Task<OasisUser?> GetUserByEmailAsync(string email);
}