using Microsoft.EntityFrameworkCore;

namespace preguntaloAPI.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; } 
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Consulta_Categoria> Consultas_Categorias { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Puntuacion> Puntuaciones { get; set; }
        public DbSet<Validacion> Validaciones { get; set; }
    }
}