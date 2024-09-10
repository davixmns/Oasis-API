using FluentValidation;
using OasisAPI.App.Dto;
using OasisAPI.App.Dto.Request;

namespace OasisAPI.App.Validators;

public class TokenRequestDtoValidator : AbstractValidator<TokenRequestDto>
{
    public TokenRequestDtoValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .MinimumLength(1)
            .WithMessage("Access token cannot be empty");
        
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .MinimumLength(1)
            .WithMessage("Refresh token cannot be empty");
    }
}