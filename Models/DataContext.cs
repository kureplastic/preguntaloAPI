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
    }
}