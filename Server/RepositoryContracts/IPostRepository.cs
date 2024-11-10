using Entities;

namespace RepositoryContracts;

public interface IPostRepository
{
    Task<Post> AddPostAsync(Post post);
    Task<Post> UpdatePostAsync(Post post);
    Task DeletePostAsync(int id);
    Task <Post> GetSinglePostAsync(int id);
    Task<IEnumerable<Post>> GetManyPostsAsync();
}