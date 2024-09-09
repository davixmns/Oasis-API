using AutoMapper;
using Domain.Entities;
using OasisAPI.Dto;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;
using OasisAPI.Utils;

namespace OasisAPI.Services;

public sealed class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<OasisApiResult<OasisUserResponseDto>> CreateUserAsync(OasisUser userData)
    {
        var userExists = await _unitOfWork.GetRepository<OasisUser>().GetAsync(u => u.Email == userData.Email);
        
        if (userExists is not null)
            return OasisApiResult<OasisUserResponseDto>.ErrorResponse("User already exists with this email");
        
        userData.Password = PasswordHasher.Hash(userData.Password);
        var userCreated = _unitOfWork.GetRepository<OasisUser>().Create(userData);
        
        await _unitOfWork.CommitAsync();
        
        var userDto = _mapper.Map<OasisUserResponseDto>(userCreated);
        
        return OasisApiResult<OasisUserResponseDto>.SuccessResponse(userDto);
    }
}