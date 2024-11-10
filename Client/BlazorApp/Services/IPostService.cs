using APIContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task AddPostAsync(CreatePostDto request);
    public Task UpdatePostAsync(int id, UpdatePostDto request);
    public Task<PostDto> GetPostAsync(int id);
    public Task<IEnumerable<PostDto>> GetPostsAsync();
    public Task DeletePostAsync(int id);
}