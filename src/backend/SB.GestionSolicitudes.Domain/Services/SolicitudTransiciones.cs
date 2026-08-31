using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Domain.Services;

public static class SolicitudTransiciones
{
    private static readonly Dictionary<EstadoSolicitudEnum, HashSet<EstadoSolicitudEnum>> TransicionesValidas = new()
    {
        {
            EstadoSolicitudEnum.Registrada,
            new HashSet<EstadoSolicitudEnum>
            {
                EstadoSolicitudEnum.EnAnalisis,
                EstadoSolicitudEnum.Cerrada
            }
        },
        {
            EstadoSolicitudEnum.EnAnalisis,
            new HashSet<EstadoSolicitudEnum>
            {
                EstadoSolicitudEnum.EnProgreso,
                EstadoSolicitudEnum.EnEsperaDelSolicitante,
                EstadoSolicitudEnum.Cerrada
            }
        },
        {
            EstadoSolicitudEnum.EnProgreso,
            new HashSet<EstadoSolicitudEnum>
            {
                EstadoSolicitudEnum.EnAnalisis,
                EstadoSolicitudEnum.EnEsperaDelSolicitante,
                EstadoSolicitudEnum.Resuelta,
                EstadoSolicitudEnum.Cerrada
            }
        },
        {
            EstadoSolicitudEnum.EnEsperaDelSolicitante,
            new HashSet<EstadoSolicitudEnum>
            {
                EstadoSolicitudEnum.EnProgreso,
                EstadoSolicitudEnum.EnAnalisis,
                EstadoSolicitudEnum.Resuelta,
                EstadoSolicitudEnum.Cerrada
            }
        },
        {
            EstadoSolicitudEnum.Resuelta,
            new HashSet<EstadoSolicitudEnum>
            {
                EstadoSolicitudEnum.Cerrada,
                EstadoSolicitudEnum.EnProgreso
            }
        },
        {
            EstadoSolicitudEnum.Cerrada,
            new HashSet<EstadoSolicitudEnum>
            {
                EstadoSolicitudEnum.EnAnalisis,
                EstadoSolicitudEnum.EnProgreso
            }
        }
    };

    public static bool EsTransicionValida(EstadoSolicitudEnum estadoActual, EstadoSolicitudEnum estadoNuevo)
    {
        if (estadoActual == estadoNuevo) return true;

        if (TransicionesValidas.TryGetValue(estadoActual, out var permitidos))
        {
            return permitidos.Contains(estadoNuevo);
        }

        return false;
    }

    public static IReadOnlyCollection<EstadoSolicitudEnum> ObtenerTransicionesPermitidas(EstadoSolicitudEnum estadoActual)
    {
        if (TransicionesValidas.TryGetValue(estadoActual, out var permitidos))
        {
            return permitidos;
        }

        return Array.Empty<EstadoSolicitudEnum>();
    }
}
