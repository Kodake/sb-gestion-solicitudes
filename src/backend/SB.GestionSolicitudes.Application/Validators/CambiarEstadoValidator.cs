using FluentValidation;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Solicitudes;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.Validators;

public class CambiarEstadoValidator : AbstractValidator<CambiarEstadoDto>
{
    public CambiarEstadoValidator()
    {
        RuleFor(x => x.NuevoEstado)
            .IsInEnum().WithMessage(MensajesSistema.Solicitud.ESTADO_INVALIDO);

        RuleFor(x => x.Comentario)
            .NotEmpty()
            .When(x => x.NuevoEstado == EstadoSolicitudEnum.Cerrada)
            .WithMessage(MensajesSistema.Solicitud.COMENTARIO_RESOLUCION_REQUERIDO)
            .MaximumLength(1000).WithMessage(MensajesSistema.Comentario.TEXTO_EXCEDE_MAXIMO);
    }
}
