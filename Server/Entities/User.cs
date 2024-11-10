namespace Entities;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }

    public User(){}

    public User(string userName, string password, string email)
    {
        Username = userName;
        Password = password;
        Email = email;
    }
}