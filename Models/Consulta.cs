namespace preguntaloAPI.Models;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Consulta{
    //*Consulta debe tener un id tipo int, 
    //*un UsuarioId de tipo int, 
    //*un Titulo de tipo string, 
    //*un Texto de tipo string, 
    //*un campo "Resuelto" de tipo boolean, 
    //*un campo "Respuesta seleccionada" de tipo int
    //*un campo "Puntuacion Positiva" de tipo int,
    //*un campo "Puntuacion Negativa" de tipo int
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }
        [Required]
        public string? Titulo {get; set;} 
        [Required]
        public string? Texto {get; set;}
        public bool Resuelto {get; set;} = false;
        public int? RespuestaSeleccionada {get; set;} = null;
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


