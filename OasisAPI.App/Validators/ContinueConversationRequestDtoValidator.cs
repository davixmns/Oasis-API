using Domain.Entities;
using FluentValidation;
using OasisAPI.App.Dto.Request;

namespace OasisAPI.App.Validators;

public class ContinueConversationRequestDtoValidator : AbstractValidator<ContinueConversationRequestDto>
{
    public ContinueConversationRequestDtoValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .MinimumLength(1)
            .WithMessage("Message cannot be empty");
        
        RuleFor(x => x.ChatBotEnums)
            .NotEmpty()
            .Must((chatBotEnums) => chatBotEnums.Count >= 1)
            .WithMessage("At least one chatbot must be selected");
        
        RuleFor(x => x.ChatBotEnums)
            .Must((chatBotEnums) => chatBotEnums.All(x => Enum.IsDefined(typeof(ChatBotEnum), x)))
            .WithMessage("Invalid chatbot enum value");
    }
    
}