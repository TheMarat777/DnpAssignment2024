using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;
public class Comment
{
    [Key] public int Id { get; set; }
    public String Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    
    [ForeignKey("UserId")] public User User { get; set; }
    [ForeignKey("PostId")] public Post Post { get; set; }
    
    public Comment(){}
    
    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }
}