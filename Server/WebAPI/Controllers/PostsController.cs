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
    private readonly ICommentRepository commentRepository;
    
    public PostsController(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }
    
    private async Task VerifyUserExistAsync(int userId)
    {
        User user = await userRepository.GetSingleUserAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with id {userId} not found");
        }
    }

    // POST https://localhost:7207/Posts
    [HttpPost]
    public async Task<IResult> CreatePost([FromBody] CreatePostDto request)
    {

        await VerifyUserExistAsync(request.UserId);

        var user = await userRepository.GetSingleUserAsync(request.UserId);

        if (user == null)
        {
            Console.WriteLine($"User with ID {request.UserId} not found.");
        }

        var post = new Post()
        {
            Title = request.Title,
            Content = request.Content,
            UserId = request.UserId,
            Likes = request.Likes
        };

        await postRepository.AddPostAsync(post);
        return Results.Created($"posts/{post.Id}", post);

    }

    // PUT https://localhost:7207/Posts/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<PostDto>> UpdatePost(int id, [FromBody] CreatePostDto request)
    {
        
        Post existingPost = await postRepository.GetSinglePostAsync(id);
        if (existingPost == null)
        {
            return NotFound();
        }
        
        await VerifyUserExistAsync(request.UserId);

        User user = await userRepository.GetSingleUserAsync(request.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        
        existingPost.Title = request.Title;
        existingPost.Content = request.Content;
        existingPost.UserId = user.Id;
        
        await postRepository.UpdatePostAsync(existingPost);
        
        PostDto postDto = new PostDto()
        {
            Id = existingPost.Id,
            Title = existingPost.Title,
            Content = existingPost.Content,
            UserId = existingPost.UserId,
            Likes = existingPost.Likes
        };

        return Ok(postDto);
    }

    
    // GET https://localhost:7207/Posts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetSinglePost(int id)
    {
        Post post = await postRepository.GetSinglePostAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        PostDto postDto = new PostDto()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            UserId = post.UserId,
            Likes = post.Likes,
        };
        return Ok(postDto);
    }
    
    // GET https://localhost:7207/Posts
    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost(CreatePostDto createPostDto)
    {
        try
        {
            var post = new Post
            {
                Title = createPostDto.Title,
                Content = createPostDto.Content,
                UserId = createPostDto.UserId
            };
            
            var addedPost = await postRepository.AddPostAsync(post);
            
            var postDto = new PostDto
            {
                Id = addedPost.Id,
                Title = addedPost.Title,
                Content = addedPost.Content,
                UserId = addedPost.UserId
            };

            return Ok(postDto);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Failed to add post: " + e.Message);
        }
    }


    
    // DELETE https://localhost:7207/Posts/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<Post>> DeletePost(int id)
    {
        Post existingPost = await postRepository.GetSinglePostAsync(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        await postRepository.DeletePostAsync(id);
        return NoContent();
    }
    
    // GET https://localhost:7207/Posts/{id}/Comments
    [HttpGet("{postId:int}/Comments")]
    public async Task<IResult> GetCommentsByPostIdAsync([FromRoute] int postId)
    {
        try
        {
            List<Comment> comments = await commentRepository.GetCommentsByPostIdAsync(postId);
            return Results.Ok(comments);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }
}