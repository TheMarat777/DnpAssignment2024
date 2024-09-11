using RepositoryContracts;

namespace CLI.UI.ManageComments;
    public class ManageCommentsView
    {
        private readonly ICommentRepository commentRepository;

        public ManageCommentsView(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
            Console.WriteLine("\nManage Comments:");
            Console.WriteLine("1. Add Comment");
            Console.WriteLine("2. List Comments");
            Console.WriteLine("3. Edit Comment");
            Console.WriteLine("4. Delete Comment");
            Console.WriteLine("0. Back to Main Menu");
            Console.WriteLine("Choose an option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await AddCommentAsync();
                    break;
                case "2":
                    await ListCommentsAsync();
                    break;
                case "3":
                    await EditCommentAsync();
                    break;
                case "4":
                    await DeleteCommentAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
            }
        }

        private async Task AddCommentAsync()
        {
            var createCommentView = new CreateCommentView(commentRepository);
            await createCommentView.ShowAsync();
        }

        private async Task ListCommentsAsync()
        {
            var listCommentsView = new ListCommentsView(commentRepository);
            await listCommentsView.ShowAsync();
        }

        private async Task EditCommentAsync()
        {
            var editCommentView = new EditCommentView(commentRepository);
            await editCommentView.ShowAsync();
        }

        private async Task DeleteCommentAsync()
        {
            var deleteCommentView = new DeleteCommentView(commentRepository);
            await deleteCommentView.ShowAsync();
        }
    }