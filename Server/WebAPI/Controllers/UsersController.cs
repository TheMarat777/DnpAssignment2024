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
        User existingUser = await userRepository.GetByUsernameAsync(userName);

        if (existingUser != null)
        {
            throw new Exception("User already exists.");
        }
    }

    // POST https://localhost:7207/Users
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto request)
    {
        try
        {
            await VerifyUserNameIsAvailableAsync(request.UserName);
            User user = new(request.UserName, request.Password);
            User createdUser = await userRepository.AddAsync(user);
            UserDto userDto = new UserDto()
            {
                Id = createdUser.Id,
                UserName = createdUser.Username
            };
            return Created($"users/{userDto.Id}", userDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500);
        }
    }
    
    // PUT https://localhost:7207/Users/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] CreateUserDto request)
    {
        User existingUser = await userRepository.GetSingleAsync(id);
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
        
        User updatedUser = await userRepository.UpdateAsync(existingUser);

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
        User user = await userRepository.GetSingleAsync(id);
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
        IEnumerable<User> users = userRepository.GetManyAsync();
        if (!string.IsNullOrEmpty(userName))
        {
            users = users.Where(u => u.Username.ToLower().Contains(userName.ToLower()));
        }
        
        List<UserDto> userDtos = new List<UserDto>();
        foreach (User user in users)
        {
            string username = (await userRepository.GetSingleAsync(user.Id)).Username;

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
        User existingUser = await userRepository.GetSingleAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }
        await userRepository.DeleteAsync(id);
        return NoContent();
    }
}

