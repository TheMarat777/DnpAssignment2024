using RepositoryContracts;

namespace CLI.UI.ManageUsers;
    public class CreateUserView
    {
        private readonly IUserRepository userRepository;

        public CreateUserView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            
            Console.Write("Enter email: ");
            string email = Console.ReadLine();
            
            var user = new Entities.User(username, password, email);
            var createdUser = await userRepository.AddUserAsync(user);
        
            Console.WriteLine($"\nUser {createdUser.Username} created successfully with ID: {createdUser.Id}");
        }
    }