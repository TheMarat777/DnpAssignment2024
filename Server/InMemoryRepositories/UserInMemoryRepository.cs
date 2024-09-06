using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    public List<User> users;
    
    //Constructor
    public UserInMemoryRepository()
    {
        users = new List<User>();
        CreateInitialUsers();
    }

    //DummyData

    private void CreateInitialUsers()
    {
        users.AddRange(new List<User>
        {
            new User{Id = 1, Password = "marat123", Username = "Marat"},
            new User{Id = 2, Password = "patrik321", Username = "Patrik"},
            new User{Id = 3, Password = "Tomas969", Username = "Tomas"},
        });
    }
    
    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any()
            ? users.Max(p => p.Id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User ? existingUser = users.SingleOrDefault(p => p.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' was not found");
        }
        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User ? userToRemove = users.SingleOrDefault(p => p.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' was not found");
        }
        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User ? user = users.SingleOrDefault(p => p.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' was not found");
        }
        return Task.FromResult(user);
        
    }

    public IQueryable<User> GetManyAsync()
    {
        return users.AsQueryable();
    }
}