using RepositoryContracts;

namespace CLI.UI.ManageComments;
    public class ListCommentsView
    {
        private readonly ICommentRepository commentRepository;

        public ListCommentsView(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task ShowAsync()
        {
            var comments = await commentRepository.GetManyAsync();
            if (comments.Any())
            {
                Console.WriteLine("\nComments: ");
                foreach (var comment in comments)
                {
                    Console.WriteLine($"ID: {comment.Id}, Body: {comment.Body}, PostID: {comment.PostId}, UserID: {comment.UserId}");
                }
            }
            else
            {
                Console.WriteLine("\nNo comments found.");
            }
        }
    }