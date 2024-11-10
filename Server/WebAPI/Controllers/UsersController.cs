using APIContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    private async Task VerifyUserNameIsAvailableAsync(string userName) //calling the user by username
    {
        User? existingUser = await userRepository.GetUserByUsernameAsync(userName);

        if (existingUser != null)
        {
            throw new Exception($"User with username {userName} already exists.");
        }   
    }

    // POST https://localhost:7207/Users
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto request)
    {
        try
        {
            await VerifyUserNameIsAvailableAsync(request.UserName);
            User user = new(request.UserName, request.Password, request.Email);
            User createdUser = await userRepository.AddUserAsync(user);
            UserDto userDto = new UserDto()
            {
                Id = createdUser.Id,
                UserName = createdUser.Username,
                Email = createdUser.Email
            };
            return Created($"users/{userDto.Id}", userDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500,  $"Internal Server Error: {e.Message}");
        }
    }
    
    // PUT https://localhost:7207/Users/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        User existingUser = await userRepository.GetSingleUserAsync(id);
        if (existingUser.Username != request.UserName)
        {
            await VerifyUserNameIsAvailableAsync(request.UserName);
        }

        if (existingUser.Username != request.UserName)
        {
            await VerifyUserNameIsAvailableAsync(request.UserName);
        }
        existingUser.Username = request.UserName;
        existingUser.Password = request.Password;
        
        User updatedUser = await userRepository.UpdateUserAsync(existingUser);

        UserDto userDto = new UserDto
        {
            Id = updatedUser.Id,
            UserName = updatedUser.Username
        };
        return Ok(userDto);
    }

    // GET https://localhost:7207/Users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetSingleUser(int id) //calling the user by id
    {
        User user = await userRepository.GetSingleUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        UserDto userDto = new UserDto()
        {
            Id = user.Id,
            UserName = user.Username
        };
        return Ok(userDto);
    }

    // GET https://localhost:7207/Users?userName=Marat
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetManyUsers([FromQuery] string? userName)
    {
        IEnumerable<User> users = userRepository.GetManyUsersAsync();
        if (!string.IsNullOrEmpty(userName))
        {
            users = users.Where(u => u.Username.ToLower().Contains(userName.ToLower()));
        }
        
        List<UserDto> userDtos = new List<UserDto>();
        foreach (User user in users)
        {
            string username = (await userRepository.GetSingleUserAsync(user.Id)).Username;

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                UserName = username
            };
            userDtos.Add(userDto);
        }
        return Ok(userDtos);
    }

    // DELETE https://localhost:7207/Users/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDto>> DeleteUser(int id)
    {
        User existingUser = await userRepository.GetSingleUserAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }
        await userRepository.DeleteUserAsync(id);
        return NoContent();
    }
}

