using preguntaloAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace preguntaloAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuariosController : Controller
    {
        private readonly DataContext contexto;

        public UsuariosController(DataContext contexto){
            this.contexto = contexto;
        }
        
        // GET: UsuariosController
        [HttpGet("{id}")]
        public Usuario? GetUsuario(int id) {
            return contexto.Usuarios.Find(id);
        }
    }

