using FluentValidation;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.EntidadesGubernamentales;

namespace SB.GestionSolicitudes.Application.Validators;

public class CrearEntidadGubernamentalValidator : AbstractValidator<CrearEntidadGubernamentalDto>
{
    public CrearEntidadGubernamentalValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage(MensajesSistema.EntidadGubernamentalMensajes.NOMBRE_REQUERIDO)
            .MaximumLength(250);

        RuleFor(x => x.Categoria)
            .NotEmpty().WithMessage(MensajesSistema.EntidadGubernamentalMensajes.CATEGORIA_REQUERIDA)
            .MaximumLength(150);

        RuleFor(x => x.PoderEstado)
            .NotEmpty().WithMessage(MensajesSistema.EntidadGubernamentalMensajes.PODER_ESTADO_REQUERIDO)
            .MaximumLength(100);

        RuleFor(x => x.Sector)
            .NotEmpty().WithMessage(MensajesSistema.EntidadGubernamentalMensajes.SECTOR_REQUERIDO)
            .MaximumLength(150);

        RuleFor(x => x.Siglas).MaximumLength(50);
        RuleFor(x => x.Direccion).MaximumLength(300);
        RuleFor(x => x.Telefono).MaximumLength(50);
        RuleFor(x => x.SitioWeb).MaximumLength(200);
    }
}

public class ActualizarEntidadGubernamentalValidator : AbstractValidator<ActualizarEntidadGubernamentalDto>
{
    public ActualizarEntidadGubernamentalValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage(MensajesSistema.EntidadGubernamentalMensajes.NOMBRE_REQUERIDO)
            .MaximumLength(250);

        RuleFor(x => x.Categoria)
            .NotEmpty().WithMessage(MensajesSistema.EntidadGubernamentalMensajes.CATEGORIA_REQUERIDA)
            .MaximumLength(150);

        RuleFor(x => x.PoderEstado)
            .NotEmpty().WithMessage(MensajesSistema.EntidadGubernamentalMensajes.PODER_ESTADO_REQUERIDO)
            .MaximumLength(100);

        RuleFor(x => x.Sector)
            .NotEmpty().WithMessage(MensajesSistema.EntidadGubernamentalMensajes.SECTOR_REQUERIDO)
            .MaximumLength(150);

        RuleFor(x => x.Siglas).MaximumLength(50);
        RuleFor(x => x.Direccion).MaximumLength(300);
        RuleFor(x => x.Telefono).MaximumLength(50);
        RuleFor(x => x.SitioWeb).MaximumLength(200);
    }
}
