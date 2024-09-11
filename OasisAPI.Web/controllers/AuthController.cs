using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.App.Commands;
using OasisAPI.App.Dto.Request;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        var command = new LoginCommand(dto.Email, dto.Password);

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }

    [Authorize]
    [HttpGet("VerifyAccessToken")]
    public async Task<IActionResult> GetOasisUserData()
    {
        var userId = int.Parse(HttpContext.Items["UserId"]!.ToString()!);
        
        var command = new GetUserDataQuery(userId);

        var result = await _mediator.Send(command);
        
        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }
}