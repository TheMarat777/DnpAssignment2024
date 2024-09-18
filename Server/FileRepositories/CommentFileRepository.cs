using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

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
        int maxId = comments.Count > 0 ? comments.Max(x => x.Id) : 1;
        comment.Id = maxId + 1;
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

    public IQueryable<Comment> GetManyAsync()
    {
        try
        {
            string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson) !;
            return comments.AsQueryable();
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

    public async Task UpdateAsync(Comment comment)
    {
        var comments = await LoadCommentsAsync();
        var existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);

        if (existingComment != null)
        {
            existingComment.Body = comment.Body;
            existingComment.UserId = comment.UserId;
            existingComment.PostId = comment.PostId;
        }
        else
        {
            throw new InvalidDataException($"Comment with id {comment.Id} could not be found.");
        }
        
    }
}