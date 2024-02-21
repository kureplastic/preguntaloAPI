namespace preguntaloAPI.Models;
using System.ComponentModel.DataAnnotations;

public class Usuario{
    //*usuario debe tener un id, un Apellido, un Nombre, un Email, Password, un campo RatingId que es clave foranea, un campo FotoPerfil que es un string, y por ultimo un campo ValidacionId que es una clave foranea *//
        public int Id { get; set; }
        [Required]
        public string Apellido { get; set; } = "";
        [Required]
        public string Nombre { get; set; } ="";
        public string Email { get; set; } ="";
        public string Password { get; set; } ="";
        public int RatingId { get; set; }
        public string? FotoPerfil { get; set; }
        public IFormFile? AvatarFile { get; set; }
        public int ValidacionId { get; set; }
        

}
