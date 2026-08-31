namespace SB.GestionSolicitudes.Application.DTOs.Comentarios;

public class CrearComentarioDto
{
    public string Texto { get; set; } = string.Empty;
    public bool EsPublico { get; set; } = true;
}
