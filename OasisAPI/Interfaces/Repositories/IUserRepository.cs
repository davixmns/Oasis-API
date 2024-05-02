using OasisAPI.Models;

namespace OasisAPI.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<OasisUser>
{
    Task<OasisUser?> GetUserByEmailAsync(string email);
}