using APIContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    public Task UpdateCommentAsync(int id, UpdateCommentDto request);
    public Task<CommentDto> GetCommentAsync(int id);
    public Task <IEnumerable<CommentDto>> GetCommentsAsync(int postId); //retrieve comments for a specific post 
    public Task DeleteCommentAsync(int id);
}