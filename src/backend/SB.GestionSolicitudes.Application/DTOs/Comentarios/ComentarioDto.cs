namespace SB.GestionSolicitudes.Application.DTOs.Comentarios;

public class ComentarioDto
{
    public int Id { get; set; }
    public int SolicitudId { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string UsuarioRol { get; set; } = string.Empty;
    public string Texto { get; set; } = string.Empty;
    public bool EsPublico { get; set; }
    public DateTime Fecha { get; set; }
}
