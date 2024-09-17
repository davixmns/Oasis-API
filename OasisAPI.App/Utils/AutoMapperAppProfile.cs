using AutoMapper;
using Domain.Entities;
using OasisAPI.App.Features.User.Queries.GetUserData;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Mapper;

public class AutoMapperAppProfile : Profile
{
    public AutoMapperAppProfile()
    {
        CreateMap<OasisUser, OasisUserResponseDto>();
        CreateMap<OasisUserResponseDto, OasisUser>();
    }
}