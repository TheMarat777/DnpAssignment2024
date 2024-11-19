using System.ComponentModel.DataAnnotations;

namespace Entities;
public class User
{
    [Key] public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public User(){}

    public User(string userName, string password, string email)
    {
        Username = userName;
        Password = password;
        Email = email;
    }
}