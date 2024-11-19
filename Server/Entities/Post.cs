using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities;

public class Post
{
    [Key] public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public int Likes { get; set; }
    
    [ForeignKey("UserId")] public User User { get; set; }
    public ICollection<Comment> Comments { get; set; }
    
    public Post(){}
    
    public Post(string title, string body)
    {
        Title = title;
        Content = body;
    }

    public Post(int id, string title, string content)
    {
        Id = id;
        Title = title;
        Content = content;
        Likes = 0;
    }
}