using System.Collections;

namespace APIContracts;

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public int Likes { get; set; }
}