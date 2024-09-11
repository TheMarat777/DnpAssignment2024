using RepositoryContracts;

namespace CLI.UI.ManagePosts;
    public class ManagePostsView
    {
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;

        public ManagePostsView(IPostRepository postRepository, IUserRepository userRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.WriteLine("\nManage Posts: ");
                Console.WriteLine("1. Create Post");
                Console.WriteLine("2. Update Post");
                Console.WriteLine("3. List Posts");
                Console.WriteLine("4. Post Overview");
                Console.WriteLine("5. View Specific Post");
                Console.WriteLine("6. Delete Post");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("Choose an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await CreatePostAsync();
                        break;
                    case "2":
                        await UpdatePostAsync();
                        break;
                    case "3":
                        await ListPostsAsync();
                        break;
                    case "4":
                        await PostOverviewAsync();
                        break;
                    case "5":
                        await ViewSpecificPostAsync();
                        break;
                    case "6":
                        await DeletePostAsync();
                        break;
                    case "0":
                        Console.WriteLine("Returning to Main Manu..");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        private async Task CreatePostAsync()
        {
            CreatePostView createPostView = new CreatePostView(postRepository, userRepository);
            await createPostView.ShowAsync();
        }

        private async Task UpdatePostAsync()
        {
            UpdatePostView updatePostView = new UpdatePostView(postRepository, userRepository);
            await updatePostView.ShowAsync();
        }

        private async Task ListPostsAsync()
        {
            ListPostsView listPostsView = new ListPostsView(postRepository, userRepository);
            await listPostsView.ShowAsync();
        }

        private async Task PostOverviewAsync()
        {
            OverviewPostView overviewPostView = new OverviewPostView(postRepository);
            await overviewPostView.ShowAsync();
        }

        private async Task ViewSpecificPostAsync()
        {
            SinglePostView singlePostView = new SinglePostView(postRepository, userRepository);
            await singlePostView.ShowAsync();
        }

        private async Task DeletePostAsync()
        {
            DeletePostView deletePostView = new DeletePostView(postRepository);
            await deletePostView.ShowAsync();
        }
    }