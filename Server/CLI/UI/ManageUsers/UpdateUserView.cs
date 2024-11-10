using RepositoryContracts;

namespace CLI.UI.ManageUsers;
    public class UpdateUserView
    {
        private readonly IUserRepository userRepository;

        public UpdateUserView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            Console.WriteLine("Enter the ID of the user you want to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var user = await userRepository.GetSingleUserAsync(id);
                if (user != null)
                {
                    Console.Write($"Enter new username (current: {user.Username}) ");
                    string newUsername = Console.ReadLine();
                
                    Console.Write($"Enter new password (current: {user.Password}): ");
                    string newPassword = Console.ReadLine();
                
                    user.Username = newUsername;
                    user.Password = newPassword;
                
                    await userRepository.UpdateUserAsync(user);
                    Console.WriteLine($"User with ID {id} was successfully updated.");
                }
                else
                {
                    Console.WriteLine($"User with ID {id} was not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Enter a valid user ID.");
            }
        }   
    }