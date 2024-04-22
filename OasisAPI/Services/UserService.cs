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
    
    public async Task<OasisUserDto> CreateUser(OasisUser userData)
    {
        var userCreated = await _userRepository.CreateUserAsync(userData);
        var userDto = _mapper.Map<OasisUserDto>(userCreated);
        return userDto;
    }
}