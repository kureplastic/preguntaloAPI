namespace preguntaloAPI.Models;

public class Validacion{
    //validacion tiene un id de tipo int, un Titulo, una entidadOtorgante y una descripcion de tipo String y un confirmada de tipo boolean
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string EntidadOtorgante { get; set; }
    public string Descripcion { get; set; }
    public bool Confirmada { get; set; }
}