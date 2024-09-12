using AutoMapper;
using Domain.Entities;
using Domain.utils;
using OasisAPI.Infra.Dto;
using OpenAI.Threads;

namespace OasisAPI.Infra.Mapper;

public class AutoMapperInfraProfile : Profile
{
    public AutoMapperInfraProfile()
    {
        //ChatGPTMessage -> ChatBotMessageResponseDto
        CreateMap<MessageResponse, ChatBotMessageResponseDto>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Content[0].Text.ToString()!))
            .ForMember(dest => dest.ChatBotName, opt => opt.MapFrom(FromNames.ChatGpt))
            .ForMember(dest => dest.ThreadId, opt => opt.MapFrom(src => src.ThreadId))
            .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.Id));
        
        //GeminiMessage -> ChatBotMessageResponseDto
        CreateMap<string, ChatBotMessageResponseDto>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.ChatBotName, opt => opt.MapFrom(FromNames.Gemini))
            .ForMember(dest => dest.ThreadId, opt => opt.Ignore()) //Gemini doesn't have threads
            .ForMember(dest => dest.MessageId, opt => opt.Ignore()); //Gemini doesn't have message ids
    }
}