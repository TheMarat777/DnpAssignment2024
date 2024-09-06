using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    public List<Post> posts;
    
    //Constructor

    public PostInMemoryRepository()
    {
        posts = new List<Post>();
        CreateInitialPost();
    }
    
    //DummyData

    private void CreateInitialPost()
    {
        posts.AddRange(new List<Post>
        {
            new Post{Body = "Just finished an intense workout session! Feeling stronger every day. Stay consistent, and the results will follow.", Title = "Fitness", Id = 1, UserId = 1},
            new Post{Body = "Tried out this new recipe today, and it was a hit! Nothing beats a home-cooked meal with fresh ingredients.", Title = "Recipe", Id = 2, UserId = 2},
            new Post{Body = "Excited to announce our latest update! We've enhanced the user experience with smoother navigation and faster load times. Stay tuned for more!", Title = "Updates", Id = 3, UserId = 3}
        });
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post ? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' was not found");
        }
        posts.Remove(existingPost);
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' was not found");
        }
        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' was not found");
        }
        return Task.FromResult(post);
        
    }

    public IQueryable<Post> GetManyAsync()
    {
        return posts.AsQueryable();
    }
}