using RepositoryContracts;

namespace CLI.UI.ManageComments;
    public class EditCommentView
    {
        private readonly ICommentRepository commentRepository;

        public EditCommentView(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task ShowAsync()
        {
            Console.Write("Enter the ID of the comment to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int commentId))
            {
                Console.WriteLine("Invalid comment ID.");
                return;
            }

            var comment = await commentRepository.GetSingleAsync(commentId);
            if (comment == null)
            {
                Console.WriteLine($"Comment with ID {commentId} not found.");
                return;
            }

            Console.WriteLine($"Editing Comment (Current Body: {comment.Body})");
            Console.Write("Enter new comment body: ");
            string newBody = Console.ReadLine();

            comment.Body = newBody;
            await commentRepository.UpdateAsync(comment);

            Console.WriteLine($"Comment with ID {commentId} updated successfully.");
        }
    }