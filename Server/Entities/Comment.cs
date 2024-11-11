namespace Entities;
public class Comment
{
    public int Id { get; set; }
    public String Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    
    public Comment(){}
    
    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }
}