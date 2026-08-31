using FluentValidation;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Auth;

namespace SB.GestionSolicitudes.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Correo)
            .NotEmpty().WithMessage(MensajesSistema.Auth.CORREO_REQUERIDO)
            .EmailAddress().WithMessage(MensajesSistema.Auth.CORREO_INVALIDO);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(MensajesSistema.Auth.PASSWORD_REQUERIDA);
    }
}
