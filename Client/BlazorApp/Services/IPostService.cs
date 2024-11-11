using System.Collections;
using APIContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task AddPostAsync(CreatePostDto request);
    public Task UpdatePostAsync(int id, UpdatePostDto request);
    public Task<PostDto> GetPostAsync(int id);
    public Task<IEnumerable<PostDto>> GetPostsAsync();
    public Task DeletePostAsync(int id);
    public Task<PostWithCommentsDTO> GetPostByIdAsync(int id);
    public Task<List<CommentDto>> GetCommentsAsync(int postId);
    public Task<CreateCommentDto> AddCommentAsync(CreateCommentDto dto);
}