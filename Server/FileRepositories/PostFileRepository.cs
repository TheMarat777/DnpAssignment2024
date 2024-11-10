using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    private async Task<List<Post>> LoadPostsAsync()
    {
        try
        {
            string postsAsJson = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Post>>(postsAsJson) ?? new List<Post>();
        }
        catch (JsonException e)
        {
            throw new InvalidOperationException("Could not deserialize posts.json", e);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("An error occured while loading posts", e);
        }
    }

    private async Task SavePostsAsync(List<Post> posts)
    {
        try
        {
            string postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, postsAsJson);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("An error occured while saving posts", e);
        }
    }

    public async Task<Post> AddPostAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson) !;
        post.Id = posts.Count > 0 ? posts.Max(p => p.Id) +1 : 1;
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
    }

    public async Task DeletePostAsync(int postId)
    {
        var posts = await LoadPostsAsync();
        var post = posts.SingleOrDefault(p => p.Id == postId);

        if (post != null)
        {
            posts.Remove(post);
            await SavePostsAsync(posts);
        }
        else
        {
            throw new InvalidOperationException($"Post with id {postId} was not found");
        }
    }

    public async Task<Post> GetSinglePostAsync(int postId)
    {
        var posts = await LoadPostsAsync();
        var post = posts.FirstOrDefault(p => p.Id == postId);

        if (post != null)
        {
            return post;
        }
        else
        {
            throw new InvalidOperationException($"Post with id {postId} was not found");
        }
    }

    public async Task<IEnumerable<Post>> GetManyPostsAsync()
    {
        try
        {
            string postsAsJson = File.ReadAllTextAsync(filePath).Result;
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson) !;
            return posts.AsQueryable();
        }
        catch(JsonException e)
        {
            throw new InvalidOperationException("Could not deserialize posts.json", e);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("An error occured while loading posts", e);
        }
    }

    public async Task<Post> UpdatePostAsync(Post post)
    {
        var posts = await LoadPostsAsync();
        var existingPost = posts.SingleOrDefault(p => p.Id == post.Id);

        if (existingPost != null)
        {
            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.UserId = post.UserId;
            
            await SavePostsAsync(posts);
        }
        else
        {
            throw new InvalidOperationException($"Post with id {post.Id} was not found");
        }
        return existingPost;
    }
}