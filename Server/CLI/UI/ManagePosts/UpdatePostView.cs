using RepositoryContracts;

namespace CLI.UI.ManagePosts;
    public class UpdatePostView
    {
        private readonly IUserRepository userRepository;
        private readonly IPostRepository postRepository;

        public UpdatePostView(IPostRepository postRepository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.postRepository = postRepository;
        }

        public async Task ShowAsync()
        {
            Console.WriteLine("Enter the ID of the post you want to update:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var post = await postRepository.GetSingleAsync(id);
                    if (post != null)
                    {
                        Console.WriteLine($"Enter new title (current: {post.Title}): ");
                        string newTitle = Console.ReadLine();
                        newTitle = string.IsNullOrWhiteSpace(newTitle) ? post.Title : newTitle;

                        Console.WriteLine($"Enter new body (current: {post.Content}): ");
                        string newBody = Console.ReadLine();
                        newBody = string.IsNullOrWhiteSpace(newBody) ? post.Content : newBody;

                        Console.WriteLine($"Enter new User ID (current: {post.UserId}): ");
                        string userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int newUserId))
                        {
                            var user = await userRepository.GetSingleAsync(newUserId);

                            post.Title = newTitle;
                            post.Content = newBody;
                            post.UserId = newUserId;

                            await postRepository.UpdateAsync(post);
                            Console.WriteLine($"Post with id {post.Id} successfully updated.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid User ID. Try again.");
                    }
                }
                catch (InvalidCastException e)
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