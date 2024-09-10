using Domain.Entities;
using FluentValidation;
using OasisAPI.App.Dto;
using OasisAPI.App.Dto.Request;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Validators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address");
        
        RuleFor(x => x.Email)
            .Must((email) =>
            {
                var userExists = unitOfWork.GetRepository<OasisUser>().GetAsync(u => u.Email == email).Result;
                return userExists != null;
            })
            .WithMessage("User does not exist");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(1);
    }
}