namespace preguntaloAPI.Models;

using System.ComponentModel.DataAnnotations;

public class Consulta_Categoria {
    //Consulta_Categoria tiene un Id, una Consulta, y una Categoria

public int Id { get; set; }
[Required]
public int ConsultaId { get; set; }
public Consulta? Consulta { get; set; }
[Required]
public int CategoriaId { get; set; }
public Categoria? Categoria { get; set; }

}