using Domain.Entities;
using Domain.Utils;
using FluentValidation;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Features.Message.Commands.CreateOasisMessage;

public class CreateOasisMessageRequestDtoValidator : AbstractValidator<CreateOasisMessageRequestDto>
{
    public CreateOasisMessageRequestDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .MinimumLength(1)
            .WithMessage("This message is too short");
        
        RuleFor(x => x.ChatBotEnum)
            .NotEmpty()
            .Must(x => Enum.IsDefined(typeof(ChatBotEnum), x))
            .WithMessage("Invalid chat bot enum");

        RuleFor(x => x.OasisChatId)
            .NotEmpty()
            .Must((oasisChatId) =>
            {
                var chatExists = unitOfWork.GetRepository<OasisChat>().GetAsync(x => x.Id == oasisChatId).Result;
                return chatExists != null;
            })
            .WithMessage("This chat does not exist");

    }
}