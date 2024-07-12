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
        //Metodos Necesarios
        //     - Crear Consulta
        //     - Obtener consulta
        //     - Editar consulta
        //     - Resolver consulta
        //     - Seleccionar respuesta correcta
        //     - Eliminar Consulta
        
        //** POST: Consultas/Crear
        [HttpPost("Crear")]
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

        //** PUT: Consultas/Editar
        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] Consulta editada)
        {
           //obtener la consulta original
           var original = contexto.Consultas.Where(consulta => consulta.Id == editada.Id).FirstOrDefault();
           if(original == null) {
            return BadRequest("Consulta no encontrada");
            }
            //modificar los parametros
           original.Titulo = editada.Titulo;
           original.Texto = editada.Texto;
           //guarda en db
           try{
                await contexto.SaveChangesAsync();
                var modificado = contexto.Consultas.Where(consulta => consulta.Id == editada.Id).FirstOrDefault();
                return Ok(modificado);
           }catch(Exception ex){
            return BadRequest(ex.Message);
           }
        }

        //** PUT: Consultas/Resolver
        [HttpPut("Resolver")]
        public async Task<IActionResult> Resolver([FromBody]Consulta editada){
            //primero obtener la consulta
            var original = contexto.Consultas.Where(consulta => consulta.Id == editada.Id).FirstOrDefault();
            if(original == null){
                return BadRequest("Consulta no encontrada");
            }
            //comprobar que la consulta le pertence al usuario?
            //luego invertir valor consulta.Resuelto
            original.Resuelto = !original.Resuelto;
            try{
                await contexto.SaveChangesAsync();
                var modificado = contexto.Consultas.Where(consulta => consulta.Id == editada.Id).FirstOrDefault();
                return Ok(modificado);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //** DELETE: Consultas/Eliminar
        [HttpDelete("Eliminar")]
        public async Task<IActionResult> Delete([FromBody] Consulta eliminar){

            var consulta = await contexto.Consultas.FindAsync(eliminar.Id);
            if (consulta == null)
					return NotFound();            
            try{
                    contexto.Consultas.Remove(consulta);
					await contexto.SaveChangesAsync();
					return Ok("Consulta eliminada");
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //** GET: Consultas/Obtener/5
        [HttpGet("Obtener/{Id}")]
        public async Task<IActionResult> Obtener(int Id){
            var emailUsuario = User.Identity.Name;
            var consultaLlena = await contexto.Consultas.Include(consulta => consulta.Usuario).Where(consulta => consulta.Id == Id &&  (consulta as Consulta).Usuario.Id == consulta.UsuarioId ).FirstOrDefaultAsync();  
            try{
                if(consultaLlena !=null){
                    consultaLlena.Usuario.Password = null;
                }
                return consultaLlena != null ? Ok(consultaLlena) : BadRequest("Consulta no encontrada");
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //** GET: Consultas/ObtenerTodas
        [HttpGet("ObtenerTodas")]
        public async Task<IActionResult> ObtenerTodas(){
            var emailUsuario = User.Identity.Name;
            try{
                var consultas = await contexto.Consultas.Where(consulta => consulta.Texto != null).OrderByDescending(consulta => consulta.Id).ToListAsync();
                return consultas != null ? Ok(consultas) : BadRequest("No hay consultas con este criterio");
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //** GET: Consultas/ObtenerPorTitulo
        [HttpPost("ObtenerPorTitulo")]
        public async Task<IActionResult> ObtenerPorTitulo([FromForm] string busqueda){
            var emailUsuario = User.Identity.Name;
            try{
                var consultas = await contexto.Consultas.Where(consulta => consulta.Texto.Contains(busqueda)).ToListAsync();
                return consultas != null ? Ok(consultas) : BadRequest("No hay consultas con este criterio");
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //** GET: Consultas/ObtenerPorUsuario
        [HttpGet("ObtenerPorUsuario")]
        public async Task<IActionResult> ObtenerPorUsuario(){
            var emailUsuario = User.Identity.Name;
            try{
                var consultas = await contexto.Consultas.Include(consulta => consulta.Usuario).Where(consulta => consulta.Usuario.Email == emailUsuario && consulta.Texto != null).OrderByDescending(consulta => consulta.Id).ToListAsync();
                return consultas != null ? Ok(consultas) : BadRequest("No hay consultas con este criterio");
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
