using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task<User> GetSingleUserAsync(int id);
    Task<User> GetUserByUsernameAsync(string userName); //username specific retrieval
    IQueryable<User> GetManyUsersAsync();
}