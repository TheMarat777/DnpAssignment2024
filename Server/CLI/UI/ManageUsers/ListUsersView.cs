using RepositoryContracts;

namespace CLI.UI.ManageUsers;
    public class ListUsersView
    {
        private readonly IUserRepository userRepository;

        public ListUsersView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            var users = await userRepository.GetManyAsync();
            if (users.Any())
            {
                Console.WriteLine("\nUser list: ");
                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.Id}, Username: {user.Username}");
                }
            }
            else
            {
                Console.WriteLine("\nNo users found.");
            }
        }
    }