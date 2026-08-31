using FluentValidation;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Solicitudes;

namespace SB.GestionSolicitudes.Application.Validators;

public class CrearSolicitudValidator : AbstractValidator<CrearSolicitudDto>
{
    public CrearSolicitudValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage(MensajesSistema.Solicitud.TITULO_REQUERIDO)
            .MaximumLength(150).WithMessage(MensajesSistema.Solicitud.TITULO_EXCEDE_MAXIMO);

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage(MensajesSistema.Solicitud.DESCRIPCION_REQUERIDA)
            .MaximumLength(2000).WithMessage(MensajesSistema.Solicitud.DESCRIPCION_EXCEDE_MAXIMO);

        RuleFor(x => x.AreaId)
            .GreaterThan(0).WithMessage(MensajesSistema.Solicitud.AREA_INVALIDA);

        RuleFor(x => x.TipoSolicitudId)
            .GreaterThan(0).WithMessage(MensajesSistema.Solicitud.TIPO_SOLICITUD_INVALIDO);

        RuleFor(x => x.Prioridad)
            .IsInEnum().WithMessage(MensajesSistema.Solicitud.PRIORIDAD_INVALIDA);

        RuleFor(x => x.ReferenciaEvidencia)
            .MaximumLength(500).WithMessage(MensajesSistema.Solicitud.REFERENCIA_EVIDENCIA_EXCEDE_MAXIMO);
    }
}
