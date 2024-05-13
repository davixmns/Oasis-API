using Microsoft.AspNetCore.Mvc;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    
    public UserController(IUserService userService)
    {
        this.userService = userService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] OasisUser userData)
    {
        var createUserResult = await this.userService
            .CreateUserAsync(userData);
        
        if (!createUserResult.Success)
            return BadRequest(createUserResult);
        
        return Ok(createUserResult);
    }
}

