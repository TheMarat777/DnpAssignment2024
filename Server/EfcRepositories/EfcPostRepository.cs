using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext ctx;

    public EfcPostRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Post> AddPostAsync(Post post)
    {
        bool userExists = await ctx.Users.AnyAsync(u => u.Id == post.UserId);
        if (!userExists)
        {
            throw new ArgumentException($"User with id {post.UserId} not found.");
        }
        
        EntityEntry<Post> entityEntry = await ctx.Posts.AddAsync(post);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdatePostAsync(Post post)
    {
        if (!(await ctx.Posts.AnyAsync(p => p.Id == post.Id)))
        {
            throw new ArgumentException($"Post with id {post.Id} not found");
        }
        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
    }

    public async Task DeletePostAsync(int id)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new ArgumentException($"Post with id {id} not found");
        }
        ctx.Posts.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<Post> GetSinglePostAsync(int id)
    {
        Post? post = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        return post;
    }

    public async Task<IEnumerable<Post>> GetManyPostsAsync()
    {
        return ctx.Posts.AsEnumerable();
    }
}