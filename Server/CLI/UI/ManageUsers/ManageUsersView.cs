using RepositoryContracts;

namespace CLI.UI.ManageUsers;
    public class ManageUsersView
    {
        private readonly IUserRepository userRepository;

        public ManageUsersView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        { 
            while (true) 
            { 
                Console.WriteLine("\nManage users:");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. List Users");
                Console.WriteLine("3. Update User");
                Console.WriteLine("4. Delete User");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: "); 
                string input = Console.ReadLine();
            
                switch (input)
                {
                    case "1":
                        await CreateUserAsync();
                        break;
                    case "2":
                        await ListUsersAsync();
                        break;
                    case "3":
                        await UpdateUserAsync();
                        break;
                    case "4":
                        await DeleteUserAsync();
                        break;
                    case "0":
                        Console.WriteLine("Exiting..");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        private async Task CreateUserAsync()
        {
            CreateUserView createUserView = new CreateUserView(userRepository);
            await createUserView.ShowAsync();
        }

        private async Task ListUsersAsync()
        {
            ListUsersView listUsersView = new ListUsersView(userRepository);
            await listUsersView.ShowAsync();
        }

        private async Task UpdateUserAsync()
        {
            UpdateUserView updateUserView = new UpdateUserView(userRepository);
            await updateUserView.ShowAsync();
        }

        private async Task DeleteUserAsync()
        {
            DeleteUserView deleteUserView = new DeleteUserView(userRepository);
            await deleteUserView.ShowAsync();
        }
    }