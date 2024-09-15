using AutoMapper;
using Domain.Entities;
using OasisAPI.App.Dto.Request;
using OasisAPI.App.Dto.Response;
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