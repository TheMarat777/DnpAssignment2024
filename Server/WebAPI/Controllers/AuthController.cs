using APIContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;
    
    private readonly IPostRepository postRepository;

    public AuthController(IUserRepository userRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }

    [HttpPost("auth/register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var user = await userRepository.GetUserByUsernameAsync(registerRequest.Username);
        if (user != null)
        {
            return Conflict("Username is already taken.");
        }

        var newUser = new User
        {
            Username = registerRequest.Username,
            Password = registerRequest.Password,
            Email = registerRequest.Email
        };
        await userRepository.AddUserAsync(newUser);

        var userDto = new UserDto
        {
            Id = newUser.Id,
            UserName = newUser.Username,
            Email = newUser.Email
        };
        return CreatedAtAction(nameof(Register), new { id = newUser.Id }, userDto);
    }

    [HttpPost("auth/login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await userRepository.GetUserByUsernameAsync(loginRequest.Username);
        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        if (user.Password != loginRequest.Password)
        {
            return Unauthorized("Invalid username or password");
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.Username,
            Email = user.Email
        };
        return Ok(userDto);
    }
    
    [HttpPost("auth/adduser")]
    public async Task<ActionResult> AddUser([FromBody] RegisterRequest registerRequest)
    {
        var user = await userRepository.GetUserByUsernameAsync(registerRequest.Username);
        if (user != null)
        {
            return Conflict("Username is already taken.");
        }

        var newUser = new User
        {
            Username = registerRequest.Username,
            Password = registerRequest.Password,
            Email = registerRequest.Email
        };
        await userRepository.AddUserAsync(newUser);

        var userDto = new UserDto
        {
            Id = newUser.Id,
            UserName = newUser.Username,
            Email = newUser.Email
        };
        return CreatedAtAction(nameof(AddUser), new { id = newUser.Id }, userDto);
    }
    
}