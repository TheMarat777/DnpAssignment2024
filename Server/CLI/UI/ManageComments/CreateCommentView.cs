using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;
    public class CreateCommentView
    {
        private readonly ICommentRepository commentRepository;

        public CreateCommentView(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task ShowAsync()
        {
            Console.WriteLine("Enter your comment:");
            string body = Console.ReadLine();
        
            Console.WriteLine("Enter the post ID you want to comment on:");
            if (!int.TryParse(Console.ReadLine(), out int postId))
            {
                Console.WriteLine("Invalid post ID.");
                return;
            }

            Console.Write("Enter user ID: ");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Invalid user ID.");
                return;
            }

            var comment = new Comment(body, postId, userId);
            var createdComment = await commentRepository.AddAsync(comment);

            Console.WriteLine($"Comment added successfully with ID: {createdComment.Id}");
        }
    }