using System.Collections;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;

    public EfcCommentRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        var userExists = await ctx.Users.AnyAsync(u => u.Id == comment.UserId);
        if (!userExists)
        {
            throw new ArgumentException($"User with ID {comment.UserId} not found");
        }
        
        var postExists = await ctx.Posts.AnyAsync(p => p.Id == comment.PostId);
        if (!postExists)
        {
            throw new ArgumentException($"Post with ID {comment.PostId} not found");
        }
        
        EntityEntry<Comment> entityEntry = await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
        
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        if (!(await ctx.Comments.AnyAsync(c => c.Id == comment.Id)))
        {
            throw new ArgumentException($"Comment with id {comment.Id} not found");
        }
        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int id)
    {
        Comment? existing = await ctx.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (existing == null)
        {
            throw new Exception($"Comment with id {id} not found");
        }
        ctx.Comments.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<Comment> GetSingleCommentAsync(int id)
    {
        Comment? comment = await ctx.Comments.SingleOrDefaultAsync(c => c.Id == id);
        
        return comment;
    }

    public async Task<IEnumerable<Comment>> GetManyCommentsAsync()
    {
        return await ctx.Comments.ToListAsync();
    }

    public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        return await ctx.Comments.Where(c => c.PostId == postId).ToListAsync();
    }
}