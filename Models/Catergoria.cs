namespace preguntaloAPI.Models;
using System.ComponentModel.DataAnnotations;

public class Categoria{
    //Categoria tiene un nombre y un ID

    public int Id {get; set;}
    public string Nombre {get; set;} = "";
}