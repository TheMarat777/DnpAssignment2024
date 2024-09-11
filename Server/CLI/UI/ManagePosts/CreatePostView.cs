using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

    public class CreatePostView
    {
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;

        public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
        {
            this.postRepository = postRepository;   
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            Console.Write("Enter post title: ");
            string title = Console.ReadLine();
        
            Console.Write("Enter post body: ");
            string body = Console.ReadLine();
        
            Console.Write("Enter User ID of the author: ");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                try
                {
                    var user = await userRepository.GetSingleAsync(userId);

                    var post = new Post(title, body, userId);
                    var createdPost = await postRepository.AddAsync(post);

                    Console.WriteLine($"Post: '{createdPost.Title}' created successfully with ID: {createdPost.Id}");
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid User ID. Enter a valid integer.");
            }
        }
    }