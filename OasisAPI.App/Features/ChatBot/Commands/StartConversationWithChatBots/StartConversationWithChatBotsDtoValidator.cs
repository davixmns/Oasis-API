using Domain.Utils;
using FluentValidation;

namespace OasisAPI.App.Features.ChatBot.Commands.StartConversationWithChatBots;

public class StartConversationWithChatBotsDtoValidator : AbstractValidator<StartConversationWithChatBotsDto>
{
    public StartConversationWithChatBotsDtoValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .MinimumLength(1)
            .WithMessage("Message cannot be empty");
    }
}