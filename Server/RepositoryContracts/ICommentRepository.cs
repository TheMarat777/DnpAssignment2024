using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Task<Comment> AddCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(int id);
    Task<Comment> GetSingleCommentAsync(int id);
    Task<IEnumerable<Comment>> GetManyCommentsAsync();
    Task<List<Comment>> GetCommentsByPostIdAsync(int postId);
}