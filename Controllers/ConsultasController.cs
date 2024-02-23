using preguntaloAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace preguntaloAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ConsultasController : Controller
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;
        public ConsultasController(DataContext contexto, IConfiguration config )
            {
                this.contexto = contexto;
                this.config = config;
            }
        //*Metodos Necesarios
        //*     - Crear Consulta
        //*     - Obtener consulta
        //*     - Editar consulta
        //*     - Eliminar Consulta
        
        // POST: Consultas/CrearConsulta
        [HttpPost("CrearConsulta")]
        public async Task<IActionResult> Crear([FromBody] Consulta nueva){
            var ClaimId = ((ClaimsIdentity) User.Identity).FindFirst("UserId");
            //primero crear una nueva consulta vacia
            var consulta = new Consulta();
            // llenar los campos necesarios
            consulta.Titulo = nueva.Titulo;
            consulta.Texto = nueva.Texto;
            consulta.UsuarioId = Int32.Parse(ClaimId.Value);
            //pegar en db
            try{
                var tareaasync = await contexto.Consultas.AddAsync(consulta);
                            contexto.SaveChanges();
                            //deveuelve la entidad
                            return Ok(tareaasync.Entity);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
