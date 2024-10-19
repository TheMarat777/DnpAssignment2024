using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private readonly List<Post> posts = new();

    public PostInMemoryRepository()
    {
        _ = AddAsync(new Post("Gym #1", "Today i had leg day, never again!.", 1)).Result;
        _ = AddAsync(new Post("Gym #2", "I almost broke my spine doing squads with weights.", 1)).Result;
        _ = AddAsync(new Post("DNP", "C# seems to be quite understandable.", 3)).Result;
        _ = AddAsync(new Post("Recipe", "Tried out this new recipe today, and it was a hit! Nothing beats a home-cooked meal with fresh ingredients.", 2)).Result;
        _ = AddAsync(new Post("A new friend", "In the morning a saw a bear hanging around my house.", 4)).Result;
        _ = AddAsync(new Post("Any books?", "Can u guys share with some captivant books?", 3)).Result;
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public async Task<Post> UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        await Task.CompletedTask;
        return post;
    }

    public Task DeleteAsync(int id)
    {
        var postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        var post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        return Task.FromResult(post);
    }

    public Task<IEnumerable<Post>> GetManyAsync()
    {
        return Task.FromResult<IEnumerable<Post>>(posts.ToList());
    }
}