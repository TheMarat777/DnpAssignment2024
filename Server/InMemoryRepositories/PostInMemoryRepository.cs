using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private readonly List<Post> posts = new();

    public PostInMemoryRepository()
    {
       // _ = AddPostAsync(new Post(1, "Gym #1", "Today I had leg day, never again!", new User("marat", "1234", "marat@gmail.com"))).Result;
       // _ = AddPostAsync(new Post(2, "Gym #2", "I almost broke my spine doing squats with weights.", new User("patrik", "4321", "patrik@gmail.com"))).Result;
       //_ = AddPostAsync(new Post(3, "DNP", "C# seems to be quite understandable.", new User("tomas", "1243", "tomas@gmail.com"))).Result;
       // _ = AddPostAsync(new Post(4, "Recipe", "Tried out this new recipe today, and it was a hit! Nothing beats a home-cooked meal with fresh ingredients.", new User("sebo", "2143", "sebo@gmail.com"))).Result;
       // _ = AddPostAsync(new Post(5, "A new friend", "In the morning, I saw a bear hanging around my house.", new User("marat", "1234", "marat@gmail.com"))).Result;
       // _ = AddPostAsync(new Post(6, "Any books?", "Can you guys share some captivating books?", new User("tomas", "1243", "tomas@gmail.com"))).Result;
    }

    public Task<Post> AddPostAsync(Post post)
    {
        post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public async Task<Post> UpdatePostAsync(Post post)
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

    public Task DeletePostAsync(int id)
    {
        var postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSinglePostAsync(int id)
    {
        var post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        return Task.FromResult(post);
    }

    public Task<IEnumerable<Post>> GetManyPostsAsync()
    {
        return Task.FromResult<IEnumerable<Post>>(posts.ToList());
    }
}
