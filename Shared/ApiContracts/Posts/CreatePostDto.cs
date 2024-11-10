namespace APIContracts;

public class CreatePostDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public int Likes { get; set; }
}