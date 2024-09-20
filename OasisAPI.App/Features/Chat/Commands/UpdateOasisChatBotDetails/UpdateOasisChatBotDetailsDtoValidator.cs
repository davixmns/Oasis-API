using FluentValidation;

namespace OasisAPI.App.Features.Chat.Commands.UpdateOasisChatBotDetails;

public class UpdateOasisChatBotDetailsDtoValidator : AbstractValidator<UpdateOasisChatBotDetailsDto>
{
    public UpdateOasisChatBotDetailsDtoValidator()
    {
        RuleFor(x => x.OasisChatBotDetailsId)
            .GreaterThan(0);
        
        RuleFor(x => x.IsActive)
            .NotNull();
    }
}