using RepositoryContracts;

namespace CLI.UI.ManagePosts;
    public class OverviewPostView
    {
        private readonly IPostRepository postRepository;

        public OverviewPostView(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        public async Task ShowAsync()
        {
            var posts = await postRepository.GetManyAsync();

            if (posts.Any())
            {
                Console.WriteLine("\nPost Overview:");
                foreach (var post in posts)
                {
                    Console.WriteLine($"ID: {post.Id}, Title: {post.Title}");
                }
            }
            else
            {
                Console.WriteLine("\nNo posts available for overview.");
            }
        }
    }