using RepositoryContracts;

namespace CLI.UI.ManagePosts;
    public class SinglePostView
    {
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;

        public SinglePostView(IPostRepository postRepository, IUserRepository userRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            Console.WriteLine("Enter the ID of the post you want to view:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var post = await postRepository.GetSingleAsync(id);
                    if (post != null)
                    {
                        var user = await userRepository.GetSingleAsync(post.UserId);
                        string username = user?.Username ?? "Marat";

                        Console.WriteLine($"\nPost Details:");
                        Console.WriteLine($"ID: {post.Id}");
                        Console.WriteLine($"Title: {post.Title}");
                        Console.WriteLine($"Body: {post.Content}");
                        Console.WriteLine($"Author ID: {post.UserId}, Username: {username}");
                    }
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Enter a valid post ID.");
            }
        }
    }