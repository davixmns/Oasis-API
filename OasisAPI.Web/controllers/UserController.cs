using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.Interfaces.Services;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public sealed class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] OasisUser userData)
    {
        var createUserResult = await _userService.CreateUserAsync(userData);
        
        if (!createUserResult.Success)
            return BadRequest(createUserResult);

        return Ok(createUserResult);
    }
}

