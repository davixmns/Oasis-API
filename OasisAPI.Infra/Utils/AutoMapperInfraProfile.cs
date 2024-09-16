using AutoMapper;
using Domain.Entities;
using Domain.Utils;
using OasisAPI.Infra.Dto;
using OpenAI.Threads;

namespace OasisAPI.Infra.Mapper;

public class AutoMapperInfraProfile : Profile
{
    public AutoMapperInfraProfile()
    {
        //ChatGPTMessage -> ChatBotMessageResponseDto
        CreateMap<MessageResponse, ChatBotMessageDto>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Content[0].Text.ToString()!))
            .ForMember(dest => dest.ChatBotEnum, opt => opt.MapFrom(src => ChatBotEnum.ChatGpt))
            .ForMember(dest => dest.ThreadId, opt => opt.MapFrom(src => src.ThreadId));
        
        //GeminiMessage -> ChatBotMessageResponseDto
        CreateMap<string, ChatBotMessageDto>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.ChatBotEnum, opt => opt.MapFrom(src => ChatBotEnum.Gemini))
            .ForMember(dest => dest.ThreadId, opt => opt.Ignore()); //Gemini doesn't have threads
    }
}