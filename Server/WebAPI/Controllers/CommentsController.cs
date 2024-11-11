using APIContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public CommentsController(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }

    private async Task VerifyUserExistAsync(int userId)
    {
        User user = await userRepository.GetSingleUserAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with id {userId} not found");
        }
    }

    // POST https://localhost:7207/Comments
    [HttpPost]
    public async Task<IResult> CreateComment([FromBody] CreateCommentDto request)
    {
        
            await VerifyUserExistAsync(request.UserId);
            var post = await postRepository.GetSinglePostAsync(request.PostId);
            var user = await userRepository.GetSingleUserAsync(request.UserId);
            
            
            if (post == null)
            {
                Console.WriteLine($"Post with ID {request.PostId} not found.");
            }

            if (user == null)
            {
                Console.WriteLine($"User with ID {request.UserId} not found.");
            }
            
            var comment = new Comment()
            {
                Body = request.Body,
                PostId = request.PostId,
                UserId = request.UserId
            };
            
            await commentRepository.AddAsync(comment);
            return Results.Created($"comments/{comment.Id}", comment);
            
    }

    // PUT https://localhost:7207/Comments/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> UpdateComment(int id, [FromBody] CreateCommentDto request)
    {
        Comment existingComment = await commentRepository.GetSingleAsync(id);
        if (existingComment.UserId != request.UserId)
        {
            await VerifyUserExistAsync(request.UserId);
        }

        await VerifyUserExistAsync(existingComment.UserId);
        existingComment.Body = request.Body;
        existingComment.UserId = request.UserId;
        existingComment.PostId = request.PostId;

        Comment updatedComment = await commentRepository.UpdateAsync(existingComment);

        CommentDto commentDto = new CommentDto()
        {
            Id = updatedComment.Id,
            Body = updatedComment.Body,
            UserId = updatedComment.UserId,
            PostId = updatedComment.PostId
        };

        return Ok(commentDto);
    }

    // GET https://localhost:7207/Comments/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetSingleComment(int id)
    {
        Comment comment = await commentRepository.GetSingleAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        CommentDto commentDto = new CommentDto()
        {
            Id = comment.Id,
            Body = comment.Body,
            UserId = comment.UserId,
            PostId = comment.PostId
        };
        return Ok(commentDto);
    }

    // GET https://localhost:7207/Comments?userId=1&postId=2
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetManyComments(
        [FromQuery] int? userId,
        [FromQuery] int? postId)
    {
        try
        {
            var comments = await commentRepository.GetManyAsync();

            var commentsDtos = comments.Select(comment => new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId
            }).ToList();

            return Ok(commentsDtos);
        }
        catch (Exception e)
        {
            return StatusCode(500, "An error occured while getting comments.");
        }
        
    }


// DELETE https://localhost:7207/Comments/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<CommentDto>> DeleteComment(int id)
    {
        Comment existingComment = await commentRepository.GetSingleAsync(id);
        if (existingComment == null)
        {
            return NotFound();
        }
        
        await commentRepository.DeleteAsync(id);
        return NoContent();
    }
}