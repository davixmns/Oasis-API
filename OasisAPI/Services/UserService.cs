using AutoMapper;
using OasisAPI.Dto;
using OasisAPI.Interfaces;
using OasisAPI.Models;

namespace OasisAPI.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<OasisApiResponse<OasisUserDto>> CreateUserAsync(OasisUser userData)
    {
        if (userData.Password.Length > 40)
            return OasisApiResponse<OasisUserDto>.ErrorResponse("Password is too long");
        
        var userExists = await _unitOfWork.UserRepository.GetUserByEmailAsync(userData.Email);
        
        if (userExists is not null)
            return OasisApiResponse<OasisUserDto>.ErrorResponse("User already exists with this email");
        
        userData.Password = BCrypt.Net.BCrypt.HashPassword(userData.Password);
        var userCreated = _unitOfWork.UserRepository.Create(userData);
        
        await _unitOfWork.CommitAsync();
        
        var userDto = _mapper.Map<OasisUserDto>(userCreated);
        
        return OasisApiResponse<OasisUserDto>.SuccessResponse(userDto);
    }
    
}