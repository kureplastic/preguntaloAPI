using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace preguntaloAPI.Models
{
    public class Puntuacion{
        //Puntuacion debe tener un id, un Usuario, una Consulta, y un valor PuntuacionPositiva de tipo boolean ademas tambien tendra una Respuesta

        [Key]
        public int Id { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }
        public int? ConsultaId { get; set; }
        [ForeignKey(nameof(ConsultaId))]
        public Consulta? Consulta { get; set; }
        public int? RespuestaId { get; set; }
        [ForeignKey(nameof(RespuestaId))]
        public Respuesta? Respuesta { get; set; }

        public bool PuntuacionPositiva {get; set;}

        public bool revisarPuntuacion(bool puntuacionEntrante){
            if(puntuacionEntrante == PuntuacionPositiva){
                return true;}
            PuntuacionPositiva = puntuacionEntrante;
            return false;
        }
    }
}