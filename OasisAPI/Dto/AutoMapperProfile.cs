using AutoMapper;
using OasisAPI.Models;
using OpenAI.Threads;

namespace OasisAPI.Dto;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<OasisUser, OasisUserDto>();
        CreateMap<OasisUserDto, OasisUser>();
        
        //ChatGPTMessage -> OasisMessage
        CreateMap<MessageResponse, OasisMessage>()
            .ForMember(dest => dest.From, opt => opt.MapFrom(src => "ChatGPT"))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Content[0].Text.Value))
            .ForMember(dest => dest.FromMessageId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FromThreadId, opt => opt.MapFrom(src => src.ThreadId));
        
        //GeminiMessage -> OasisMessage
        CreateMap<string, OasisMessage>()
            .ForMember(dest => dest.From, opt => opt.MapFrom(src => "Gemini"))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src));
    }
}