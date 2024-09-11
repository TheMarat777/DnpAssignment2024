using RepositoryContracts;
using CLI.UI.ManageUsers;
using CLI.UI.ManagePosts; 
using CLI.UI.ManageComments;

namespace CLI.UI;
    public class CliApp
    {
        private readonly IUserRepository userRepository;
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;

        public CliApp(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
        {
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
        }

        public async Task StartAsync()
        {
            Console.WriteLine("Welcome to the CLI application!");
            await ShowMainMenuAsync();
        }

        private async Task ShowMainMenuAsync()
        {
            while (true)
            {
                Console.WriteLine("\nMain Menu");
                Console.WriteLine("1. Manage Users");
                Console.WriteLine("2. Manage Posts");
                Console.WriteLine("3. Manage Comments");
                Console.WriteLine("0. Exit");
                Console.WriteLine("Select an option:");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await ManageUsersAsync();
                        break;
                    case "2":
                        await ManagePostsAsync();
                        break;
                    case "3":
                        await ManageCommentsAsync();
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        break;
                }
            }
        }

        private async Task ManageUsersAsync()
        {
            ManageUsersView manageUsersView = new ManageUsersView(userRepository);
            await manageUsersView.ShowAsync();
        }

        private async Task ManagePostsAsync()
        {
            ManagePostsView managePostsView = new ManagePostsView(postRepository, userRepository);
            await managePostsView.ShowAsync();
        }
    
        private async Task ManageCommentsAsync()
        {
            ManageCommentsView manageCommentsView = new ManageCommentsView(commentRepository);
            await manageCommentsView.ShowAsync();
        }
    }
