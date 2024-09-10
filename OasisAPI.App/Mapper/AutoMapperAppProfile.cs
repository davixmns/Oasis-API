using AutoMapper;
using Domain.Entities;
using OasisAPI.App.Dto.Response;

namespace OasisAPI.App.Mapper;

public class AutoMapperAppProfile : Profile
{
    public AutoMapperAppProfile()
    {
        CreateMap<OasisUser, OasisUserResponseDto>();
        CreateMap<OasisUserResponseDto, OasisUser>();
    }
}