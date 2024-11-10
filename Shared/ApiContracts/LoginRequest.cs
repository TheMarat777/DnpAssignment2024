namespace APIContracts;

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }

    public LoginRequest(string userName, string password)
    {
        Username = userName;
        Password = password;
    }
}