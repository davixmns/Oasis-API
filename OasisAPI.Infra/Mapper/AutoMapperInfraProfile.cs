using AutoMapper;
using Domain.Entities;
using OpenAI.Threads;

namespace OasisAPI.Infra.Mapper;

public class AutoMapperInfraProfile : Profile
{
    public AutoMapperInfraProfile()
    {
        //ChatGPTMessage -> OasisMessage
        CreateMap<MessageResponse, OasisMessage>()
            .ForMember(dest => dest.From, opt => opt.MapFrom(src => "ChatGPT"))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Content[0].Text))
            .ForMember(dest => dest.FromMessageId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FromThreadId, opt => opt.MapFrom(src => src.ThreadId))
            .ForMember(dest => dest.IsSaved, opt => opt.MapFrom(src => false));
        
        //GeminiMessage -> OasisMessage
        CreateMap<string, OasisMessage>()
            .ForMember(dest => dest.From, opt => opt.MapFrom(src => "Gemini"))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.IsSaved, opt => opt.MapFrom(src => false));
    }
}