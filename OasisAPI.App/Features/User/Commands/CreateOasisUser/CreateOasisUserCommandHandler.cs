using AutoMapper;
using Domain.Entities;
using MediatR;
using OasisAPI.App.Features.User.Queries.GetUserData;
using OasisAPI.App.Result;
using OasisAPI.Infra.Repositories;
using OasisAPI.Infra.Utils;

namespace OasisAPI.App.Features.User.Commands.CreateOasisUser;

public class CreateOasisUserCommandHandler : IRequestHandler<CreateOasisUserCommand, AppResult<OasisUserResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public CreateOasisUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
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

        return AppResult<OasisUserResponseDto>.Success(userDto);
    }
}