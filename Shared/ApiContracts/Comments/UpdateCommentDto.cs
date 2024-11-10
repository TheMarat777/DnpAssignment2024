namespace APIContracts;

public class UpdateCommentDto
{
    public string Body { get; set; } = string.Empty; //to prevent null values
    public int PostId { get; set; }
    public int UserId { get; set; }
}