using AutoMapper;
using OasisAPI.Dto;
using OasisAPI.Interfaces;
using OasisAPI.Models;

namespace OasisAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<OasisApiResponse<OasisUserDto>> CreateUserAsync(OasisUser userData)
    {
        if (userData.Password.Length > 40)
            return OasisApiResponse<OasisUserDto>.ErrorResponse("Password is too long");
        
        var userExists = await _userRepository.GetUserByEmail(userData.Email);
        
        if (userExists is not null)
            return OasisApiResponse<OasisUserDto>.ErrorResponse("User already exists with this email");
        
        var userCreated = await _userRepository.CreateUserAsync(userData);
        
        var userDto = _mapper.Map<OasisUserDto>(userCreated);
        
        return OasisApiResponse<OasisUserDto>.SuccessResponse(userDto);
    }
    
}