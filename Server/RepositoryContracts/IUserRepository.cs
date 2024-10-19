using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<User> GetSingleAsync(int id);
    Task<User> GetByUsernameAsync(string userName); //username specific retrieval
    IQueryable<User> GetManyAsync();
    
}