using FluentValidation;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Comentarios;

namespace SB.GestionSolicitudes.Application.Validators;

public class CrearComentarioValidator : AbstractValidator<CrearComentarioDto>
{
    public CrearComentarioValidator()
    {
        RuleFor(x => x.Texto)
            .NotEmpty().WithMessage(MensajesSistema.Comentario.TEXTO_REQUERIDO)
            .MaximumLength(1000).WithMessage(MensajesSistema.Comentario.TEXTO_EXCEDE_MAXIMO);
    }
}
