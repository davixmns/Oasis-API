using Domain.Entities;
using FluentValidation;
using OasisAPI.App.Dto.Request;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Validators;

public class CreateOasisUserRequestDtoValidator : AbstractValidator<CreateOasisUserRequestDto>
{
    public CreateOasisUserRequestDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Name must be at least 3 characters long");
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.Email)
            .Must((email) =>
            {
                var userExists = unitOfWork.GetRepository<OasisUser>().GetAsync(u => u.Email == email).Result;
                return userExists == null;
            })
            .WithMessage("User with this email already exists");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long");
    }
}