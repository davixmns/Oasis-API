using Microsoft.AspNetCore.Mvc;
using OasisAPI.Dto;
using OasisAPI.Interfaces;
using OasisAPI.Models;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
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
        
        return Ok(createUserResult.Data);
    }
}

