namespace APIContracts;

public class UpdatePostDto
{
    public string Title { get; set; } = string.Empty; //to prevent null values
    public string Content { get; set; } = string.Empty; //to prevent null values
}