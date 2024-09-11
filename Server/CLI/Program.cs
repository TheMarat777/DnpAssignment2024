﻿using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting CLI app..");
        IUserRepository userRepository = new UserInMemoryRepository();
        IPostRepository postRepository = new PostInMemoryRepository();
        ICommentRepository commentRepository = new CommentInMemoryRepository();
        

        CliApp cliApp = new CliApp(userRepository, postRepository, commentRepository);
        await cliApp.StartAsync();
    }
}