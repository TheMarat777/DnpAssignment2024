namespace APIContracts;

public class UpdateUserDto
{
    public required string UserName { get; set; }
    public string? Password { get; set; }
}