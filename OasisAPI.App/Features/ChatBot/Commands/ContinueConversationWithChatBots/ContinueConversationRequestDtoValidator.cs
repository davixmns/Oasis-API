using Domain.Entities;
using Domain.Utils;
using FluentValidation;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Features.ChatBot.Commands.ContinueConversationWithChatBots;

public class ContinueConversationRequestDtoValidator : AbstractValidator<ContinueConversationRequestDto>
{
    public ContinueConversationRequestDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.OasisChatId)
            .NotEmpty()
            .Must((oasisChatId) =>
            {
                var chatExists = unitOfWork.GetRepository<OasisChat>().GetAsync(c => c.Id == oasisChatId).Result;
                return chatExists != null;
            })
            .WithMessage("This chat does not exist");
        
        RuleFor(x => x.Message)
            .NotEmpty()
            .MinimumLength(1)
            .WithMessage("Message cannot be empty");
    }
    
}