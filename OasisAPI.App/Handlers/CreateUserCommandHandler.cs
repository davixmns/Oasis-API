using AutoMapper;
using Domain.Entities;
using MediatR;
using OasisAPI.App.Commands;
using OasisAPI.App.Dto.Response;
using OasisAPI.Infra.Repositories;
using OasisAPI.Infra.Utils;
using OasisAPI.Models;

namespace OasisAPI.App.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateOasisUserCommand, AppResult<OasisUserResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<OasisUserResponseDto>> Handle(CreateOasisUserCommand request, CancellationToken cancellationToken)
    {
        var hashedPassword = PasswordHasher.Hash(request.Password);
        var user = new OasisUser(request.Name, request.Email, hashedPassword);
        
        var createdUser = _unitOfWork.GetRepository<OasisUser>().Create(user);
        
        await _unitOfWork.CommitAsync();
        
        var userDto = _mapper.Map<OasisUserResponseDto>(createdUser);

        return AppResult<OasisUserResponseDto>.SuccessResponse(userDto);
    }
}