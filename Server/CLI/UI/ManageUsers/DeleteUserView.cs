using RepositoryContracts;

namespace CLI.UI.ManageUsers;
    public class DeleteUserView
    {
        private readonly IUserRepository userRepository;

        public DeleteUserView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            Console.WriteLine("Enter the ID of the user you want to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    await userRepository.DeleteAsync(id);
                    Console.WriteLine($"User with id {id} was successfully deleted.");
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else Console.WriteLine("Invalid input. Enter a valid user ID.");
        }
    }