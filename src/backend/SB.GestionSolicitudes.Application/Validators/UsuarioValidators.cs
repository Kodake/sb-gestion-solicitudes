using FluentValidation;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Usuarios;

namespace SB.GestionSolicitudes.Application.Validators;

public class CrearUsuarioValidator : AbstractValidator<CrearUsuarioDto>
{
    public CrearUsuarioValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage(MensajesSistema.UsuarioMensajes.NOMBRE_REQUERIDO)
            .MaximumLength(100);

        RuleFor(x => x.Correo)
            .NotEmpty().WithMessage(MensajesSistema.UsuarioMensajes.CORREO_REQUERIDO)
            .EmailAddress().WithMessage(MensajesSistema.Auth.CORREO_INVALIDO)
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(MensajesSistema.UsuarioMensajes.PASSWORD_REQUERIDA)
            .MinimumLength(6).WithMessage(MensajesSistema.UsuarioMensajes.PASSWORD_MINIMO);

        RuleFor(x => x.Rol)
            .IsInEnum();
    }
}

public class ActualizarUsuarioValidator : AbstractValidator<ActualizarUsuarioDto>
{
    public ActualizarUsuarioValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage(MensajesSistema.UsuarioMensajes.NOMBRE_REQUERIDO)
            .MaximumLength(100);

        RuleFor(x => x.Correo)
            .NotEmpty().WithMessage(MensajesSistema.UsuarioMensajes.CORREO_REQUERIDO)
            .EmailAddress().WithMessage(MensajesSistema.Auth.CORREO_INVALIDO)
            .MaximumLength(100);

        RuleFor(x => x.Rol)
            .IsInEnum();

        RuleFor(x => x.NuevoPassword)
            .MinimumLength(6).WithMessage(MensajesSistema.UsuarioMensajes.PASSWORD_MINIMO)
            .When(x => !string.IsNullOrEmpty(x.NuevoPassword));
    }
}
