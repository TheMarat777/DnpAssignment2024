namespace APIContracts;

public class PostWithCommentsDTO
{
    public PostDto Post { get; set; }
    public List<CommentDto> Comments { get; set; }
}