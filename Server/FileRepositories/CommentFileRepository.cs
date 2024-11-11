using System.Text.Json;
using Entities;
using RepositoryContracts;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    private async Task<List<Comment>> LoadCommentsAsync()
    {
        try
        {
            string commentsAsJson = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Comment>>(commentsAsJson) ?? new List<Comment>();
        }
        catch (JsonException e)
        {
            throw new InvalidDataException("Could not deserialize comments.json.", e);
        }
        catch (Exception e)
        {
            throw new InvalidDataException("An error occured while loading comments.", e);
        }
    }

    private async Task SaveCommentsAsync(List<Comment> comments)
    {
        try
        {
            string commentsAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(filePath, commentsAsJson);
        }
        catch (Exception e)
        {
            throw new InvalidDataException("An error occured while saving comments.", e);
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson) !;
        comment.Id = comments.Count > 0 ? comments.Max(x => x.Id) +1 : 1;
        comments.Add(comment);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
        return comment;
    }

    public async Task DeleteAsync(int commentId)
    {
        var comments = await LoadCommentsAsync();
        var comment = comments.SingleOrDefault(c => c.Id == commentId);

        if (comment != null)
        {
            comments.Remove(comment);
            await SaveCommentsAsync(comments);
        }
        else
        {
            throw new InvalidDataException($"Comment with id {commentId} could not be found.");
        }
    }

    public async Task<Comment> GetSingleAsync(int commentId)
    {
        var comments = await LoadCommentsAsync();
        var comment = comments.SingleOrDefault(c => c.Id == commentId);

        if (comment != null)
        {
            return comment;
        }
        else
        {
            throw new InvalidDataException($"Comment with id {commentId} could not be found.");
        }
    }

    public async Task<IEnumerable<Comment>> GetManyAsync()
    {
        try
        {
            string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson) !;
            return comments.AsEnumerable();
        }
        catch (JsonException e)
        {
            throw new InvalidDataException("Could not deserialize comments.json.", e);
        }
        catch (Exception e)
        {
            throw new InvalidDataException("An error occured while loading comments.", e);
        }
    }
    
    public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        List<Comment> comments = await LoadCommentsAsync();
        var filteredComments = comments.Where(c => c.PostId == postId).ToList();
        return filteredComments;
    }

    public async Task<Comment> UpdateAsync(Comment comment)
    {
        var comments = await LoadCommentsAsync();
        var existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);

        if (existingComment != null)
        {
            existingComment.Body = comment.Body;
            existingComment.UserId = comment.UserId;
            existingComment.PostId = comment.PostId;
            return existingComment;
        }
        else
        {
            throw new InvalidDataException($"Comment with id {comment.Id} could not be found.");
        }
    }
}