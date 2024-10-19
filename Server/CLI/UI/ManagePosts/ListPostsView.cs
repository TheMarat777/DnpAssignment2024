using RepositoryContracts;

namespace CLI.UI.ManagePosts;
    public class ListPostsView
    {
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;

        public ListPostsView(IPostRepository postRepository, IUserRepository userRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            var posts = await postRepository.GetManyAsync();

            if (posts.Any())
            {
                Console.WriteLine("\nPosts:");
                foreach (var post in posts)
                {
                    Console.WriteLine($"ID: {post.Id}, Title: {post.Title}, Author ID: {post.UserId}");
                }
            }
            else
            {
                Console.WriteLine("\nNo posts found");
            }
        }
    }