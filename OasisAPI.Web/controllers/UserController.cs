using MediatR;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.App.Commands;
using OasisAPI.App.Dto.Request;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public sealed class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateOasisUserRequestDto dto)
    {
        var command = new CreateOasisUserCommand(dto.Name, dto.Email, dto.Password);
        
        var createUserResult = await _mediator.Send(command);
        
        return createUserResult.IsSuccess
            ? Ok(createUserResult.Data)
            : BadRequest(createUserResult.Message);
    }
}

