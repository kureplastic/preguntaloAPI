namespace preguntaloAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Respuesta{
    //Respuesta debe tener un id tipo int, un Usuario, una Consulta, Una Respuesta, un campo Texto de tipo string, un campo "Puntuacion Positiva" de tipo int y un campo "Puntuacion Negativa" de tipo int.

    public int Id { get; set; }
    [Required]
    public int UsuarioId { get; set; }
    [ForeignKey(nameof(UsuarioId))]
    public Usuario? Usuario { get; set; }
    [Required]
    public int ConsultaId { get; set; }
    [ForeignKey(nameof(ConsultaId))]
    public Consulta? Consulta { get; set; } 
    public int? RespuestaId { get; set; } = null;
    [Required]
    public string? Texto {get; set;}
    public int PuntuacionPositiva {get; set;} = 0;
    public int PuntuacionNegativa {get; set;} = 0;

    internal void CambiarPuntuacion(bool puntuacionPositiva)
    {
        if (puntuacionPositiva)
        {
            this.PuntuacionPositiva++;
        }
        else
        {
            this.PuntuacionNegativa++;
        }
    }

    internal void EliminarPuntuacion(bool puntuacionPositiva)
    {
        if (puntuacionPositiva)
        {
            this.PuntuacionPositiva--;
        }
        else
        {
            this.PuntuacionNegativa--;
        }
    }
}