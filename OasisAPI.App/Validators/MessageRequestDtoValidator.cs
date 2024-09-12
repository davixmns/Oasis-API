using FluentValidation;
using OasisAPI.App.Dto;
using OasisAPI.App.Dto.Request;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Validators;

public class MessageRequestDtoValidator : AbstractValidator<CreateChatRequestDto>
{
    public MessageRequestDtoValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .MinimumLength(1)
            .WithMessage("Message cannot be empty");
        
        RuleFor(x => x.ChatBotEnums)
            .NotEmpty()
            .Must((chatBotEnums) => chatBotEnums.Count >= 1)
            .WithMessage("At least one chatbot must be selected");
    }
}