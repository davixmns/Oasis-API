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
    public async Task<OasisUserDto> CreateUser([FromBody] OasisUser userData)
    {
        var userCreated = await _userService.CreateUser(userData);
        
        return userCreated;
    }
}

//wrapper, success, failed, error
//oneof
//automapper
//https://medium.com/@Bigscal-Technologies/oneof-in-c-6a0900cf274e