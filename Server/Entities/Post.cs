namespace Entities;
public class Post
{
    public int Id { get; set; }
    public String Title { get; set; }
    public String Body { get; set; }
    public int UserId { get; set; }
    
    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
    }
}