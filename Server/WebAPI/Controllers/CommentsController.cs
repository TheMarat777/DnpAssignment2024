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

    public CommentsController(ICommentRepository commentRepository, IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
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
    public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentDto request)
    {
        try
        {
            await VerifyUserExistAsync(request.UserId);
            Comment comment = new Comment(request.Body, request.PostId, request.UserId);
            Comment createdComment = await commentRepository.AddAsync(comment);

            CommentDto commentDto = new CommentDto
            {
                Id = createdComment.Id,
                Body = createdComment.Body,
                UserId = createdComment.UserId,
                PostId = createdComment.PostId
            };
            return Created($"comments/{createdComment.Id}", commentDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
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
            Author = (await userRepository.GetSingleUserAsync(updatedComment.UserId)).Username
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
        IEnumerable<Comment> comments = await commentRepository.GetManyAsync();
        
        //filter by user id
        if (userId.HasValue)
        {
            comments = comments.Where(c => c.UserId == userId);
        }

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId);
        }
        
        List<CommentDto> commentDtos = new List<CommentDto>();
        foreach (Comment comment in comments)
        {
            string author  = (await userRepository.GetSingleUserAsync(comment.UserId)).Username;

            CommentDto commentDto = new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                Author = author
            };
            commentDtos.Add(commentDto);
        }
        return Ok(commentDtos);
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

