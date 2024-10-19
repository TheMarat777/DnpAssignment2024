using APIContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    
    public PostsController(IPostRepository postRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }
    
    private async Task VerifyUserExistAsync(int userId)
    {
        User user = await userRepository.GetSingleAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with id {userId} not found");
        }
    }

    // POST https://localhost:7207/Posts
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto request)
    {
        try
        {
            await VerifyUserExistAsync(request.UserId);
            Post post = new Post(request.Title, request.Content, request.UserId);
            Post createdPost = await postRepository.AddAsync(post);

            PostDto postDto = new PostDto()
            {
                Id = createdPost.Id,
                Title = createdPost.Title,
                Content = createdPost.Content,
                Author = (await userRepository.GetSingleAsync(createdPost.UserId)).Username,
                Likes = createdPost.Likes,
            };
            return Created($"posts/{postDto.Id}", createdPost);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    // PUT https://localhost:7207/Posts/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<PostDto>> UpdatePost(int id, [FromBody] CreatePostDto request)
    {
        Post existingPost = await postRepository.GetSingleAsync(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        await VerifyUserExistAsync(request.UserId);
        existingPost.Title = request.Title;
        existingPost.Content = request.Content;
        existingPost.UserId = request.UserId;
        
        Post updatedPost = await postRepository.UpdateAsync(existingPost);

        PostDto postDto = new PostDto()
        {
            Id = updatedPost.Id,
            Title = updatedPost.Title,
            Content = updatedPost.Content,
            Author = (await userRepository.GetSingleAsync(updatedPost.UserId)).Username,
            Likes = updatedPost.Likes
        };
        return Ok(postDto);
    }
    
    // GET https://localhost:7207/Posts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetSinglePost(int id)
    {
        Post post = await postRepository.GetSingleAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        PostDto postDto = new PostDto()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Author = (await userRepository.GetSingleAsync(post.UserId)).Username,
            Likes = post.Likes,
        };
        return Ok(postDto);
    }

    // GET https://localhost:7207/Posts?userId=1
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetManyPosts(
        [FromQuery] string? titleContains, 
        [FromQuery] int? userId, 
        [FromQuery] bool? topLiked)
    {
        IEnumerable<Post> posts = await postRepository.GetManyAsync();

        // titleContains query parameter
        if (!string.IsNullOrEmpty(titleContains))
        {
            posts = posts.Where(p => p.Title.ToLower().Contains(titleContains.ToLower()));
        }

        // userId query parameter
        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        // top 10 most liked posts
        if (topLiked.HasValue && topLiked.Value)
        {
            posts = posts.OrderByDescending(p => p.Likes).Take(10);
        }

        List<PostDto> postDtos = new List<PostDto>();
        foreach (Post post in posts)
        {
            string author = (await userRepository.GetSingleAsync(post.UserId)).Username;

            PostDto postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Author = author,
                Likes = post.Likes
            };

            postDtos.Add(postDto);
        }
    
        return Ok(postDtos);
    }
    
    // DELETE https://localhost:7207/Posts/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<Post>> DeletePost(int id)
    {
        Post existingPost = await postRepository.GetSingleAsync(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        await postRepository.DeleteAsync(id);
        return NoContent();
    }
}