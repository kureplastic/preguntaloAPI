
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace preguntaloAPI.Models
{
    public class Rating {
        //* Rating tiene un id tipo int, 
        //* un campo PuntuacionPositiva de tipo int,
         //* un campo PuntuacionNegativa de tipo int, 
         //* un campo RespuestasElegidas de tipo int,
         //* un campo RespuestasContestadas de tipo int,
         //* y un campo ConsultasRealizadas de tipo int

        [Key]
        public int Id { get; set; }
        public int PuntuacionPositiva {get; set;} = 0;
        public int PuntuacionNegativa {get; set;} = 0;
        public int RespuestasElegidas {get; set;} = 0;
        public int RespuestasContestadas {get; set;} = 0;
        public int ConsultasRealizadas {get; set;} = 0;
    }
}