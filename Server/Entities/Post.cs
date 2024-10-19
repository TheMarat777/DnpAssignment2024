namespace Entities;
public class Post
{
    public int Id { get; set; }
    public String Title { get; set; }
    public String Content { get; set; }
    public int UserId { get; set; }
    public int Likes { get; set; }
    
    public Post(string title, string body, int userId)
    {
        Title = title;
        Content = body;
        UserId = userId;
        Likes = 0;
    }
}