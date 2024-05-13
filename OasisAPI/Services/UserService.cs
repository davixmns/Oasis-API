using AutoMapper;
using OasisAPI.Dto;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;

namespace OasisAPI.Services;

public sealed class UserService : IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    
    public async Task<OasisApiResponse<OasisUserDto>> CreateUserAsync(OasisUser userData)
    {
        if (userData.Password.Length > 40)
        {
            return OasisApiResponse<OasisUserDto>
                .ErrorResponse("Password is too long");
        }

        var userExists = await unitOfWork
            .UserRepository
            .GetAsync(u => u.Email == userData.Email);
        
        if (userExists is not null)
        {
            return OasisApiResponse<OasisUserDto>
                .ErrorResponse("User already exists with this email");
        }
        
        userData.Password = BCrypt.Net.BCrypt.HashPassword(userData.Password);
        var userCreated = unitOfWork
            .UserRepository
            .Create(userData);

        await unitOfWork.CommitAsync();
        
        var userDto = mapper.Map<OasisUserDto>(userCreated);
        
        return OasisApiResponse<OasisUserDto>.SuccessResponse(userDto);
    }
    
}