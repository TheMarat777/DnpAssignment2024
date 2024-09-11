using RepositoryContracts;

namespace CLI.UI.ManagePosts;
    public class DeletePostView
    {
        private readonly IPostRepository postRepository;

        public DeletePostView(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        public async Task ShowAsync()
        {
            Console.WriteLine("Enter the ID of the post you want to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    await postRepository.DeleteAsync(id);
                    Console.WriteLine($"Post with ID {id} deleted successfully.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Enter a valid post ID.");
            }
        }
    }