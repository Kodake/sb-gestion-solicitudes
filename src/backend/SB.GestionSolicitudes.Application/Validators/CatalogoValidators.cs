using FluentValidation;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Catalogos;

namespace SB.GestionSolicitudes.Application.Validators;

public class CrearAreaValidator : AbstractValidator<CrearAreaDto>
{
    public CrearAreaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage(MensajesSistema.Catalogo.NOMBRE_REQUERIDO)
            .MaximumLength(100).WithMessage(MensajesSistema.Catalogo.NOMBRE_EXCEDE_MAXIMO);

        RuleFor(x => x.Descripcion)
            .MaximumLength(250).WithMessage(MensajesSistema.Catalogo.DESCRIPCION_EXCEDE_MAXIMO);
    }
}

public class ActualizarAreaValidator : AbstractValidator<ActualizarAreaDto>
{
    public ActualizarAreaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage(MensajesSistema.Catalogo.NOMBRE_REQUERIDO)
            .MaximumLength(100).WithMessage(MensajesSistema.Catalogo.NOMBRE_EXCEDE_MAXIMO);

        RuleFor(x => x.Descripcion)
            .MaximumLength(250).WithMessage(MensajesSistema.Catalogo.DESCRIPCION_EXCEDE_MAXIMO);
    }
}

public class CrearTipoSolicitudValidator : AbstractValidator<CrearTipoSolicitudDto>
{
    public CrearTipoSolicitudValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage(MensajesSistema.Catalogo.NOMBRE_REQUERIDO)
            .MaximumLength(100).WithMessage(MensajesSistema.Catalogo.NOMBRE_EXCEDE_MAXIMO);

        RuleFor(x => x.Descripcion)
            .MaximumLength(250).WithMessage(MensajesSistema.Catalogo.DESCRIPCION_EXCEDE_MAXIMO);
    }
}

public class ActualizarTipoSolicitudValidator : AbstractValidator<ActualizarTipoSolicitudDto>
{
    public ActualizarTipoSolicitudValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage(MensajesSistema.Catalogo.NOMBRE_REQUERIDO)
            .MaximumLength(100).WithMessage(MensajesSistema.Catalogo.NOMBRE_EXCEDE_MAXIMO);

        RuleFor(x => x.Descripcion)
            .MaximumLength(250).WithMessage(MensajesSistema.Catalogo.DESCRIPCION_EXCEDE_MAXIMO);
    }
}
