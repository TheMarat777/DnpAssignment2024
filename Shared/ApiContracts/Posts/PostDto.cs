using System.Collections;

namespace APIContracts;

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    public int Likes { get; set; }
}