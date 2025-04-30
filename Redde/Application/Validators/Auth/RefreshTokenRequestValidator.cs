using FluentValidation;
using Redde.Application.DTOs.Auth;

namespace Redde.Application.Validators.Auth
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Access token required!")
                .MinimumLength(20).WithMessage("Access token invalid");
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token required!")
                .MinimumLength(20).WithMessage("Refresh token invalid");
        }
    }
}
