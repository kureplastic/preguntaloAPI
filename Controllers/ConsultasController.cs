using preguntaloAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.Marshalling;


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
                            //obtener el rating del usuario logueado y aumentarle un punto a ConsultasCreadas
                            var usuario = await contexto.Usuarios.Include(usuario => usuario.Rating).Where(usuario => usuario.Id == Int32.Parse(ClaimId.Value)).FirstOrDefaultAsync();
                            usuario.Rating.ConsultasRealizadas++;
                            contexto.SaveChanges();
                            //armar la entidad junto con el usuario que la crea
                            var entidadAEnviar = await contexto.Consultas.Include(consulta => consulta.Usuario).Where(consulta => consulta.Id == tareaasync.Entity.Id).FirstOrDefaultAsync();
                            //deveuelve la entidad
                            return Ok(entidadAEnviar);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //** PUT: Consultas/Editar
        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] Consulta editada)
        {
           //obtener la consulta original
           var original = contexto.Consultas.Include(consulta => consulta.Usuario).Where(consulta => consulta.Id == editada.Id).FirstOrDefault();
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
                    var respuesta = new StringRespuesta();
                    respuesta.respuesta = "Consulta eliminada";
					return Ok(respuesta);
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
                var consultas = await contexto.Consultas.Include(consulta => consulta.Usuario).Where(consulta => consulta.Texto != null).OrderByDescending(consulta => consulta.Id).ToListAsync();
                return consultas != null ? Ok(consultas) : BadRequest("No hay consultas con este criterio");
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //** POST: Consultas/ObtenerPorTitulo
        [HttpPost("ObtenerPorTitulo")]
        public async Task<IActionResult> ObtenerPorTitulo([FromForm] string busqueda){
            var emailUsuario = User.Identity.Name;
            try{
                var consultas = await contexto.Consultas.Include(consulta => consulta.Usuario).Where(consulta => consulta.Texto.Contains(busqueda)).ToListAsync();
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

        //**POST Consultas/ObtenerPorCategoria
        [HttpPost("ObtenerPorCategoria")]
        public async Task<IActionResult> ObtenerPorCategoria([FromBody] string busqueda){
            var emailUsuario = User.Identity.Name;
            try{
                var consultas_categoria = await contexto.Consultas_Categorias.Where(consultas_categoria => consultas_categoria.Categoria.Nombre == busqueda).ToListAsync();
                var consultas = await contexto.Consultas.Include(consulta => consulta.Usuario).Where(consulta => consultas_categoria.Select(x => x.ConsultaId).Contains(consulta.Id)).ToListAsync();
                return consultas != null ? Ok(consultas) : BadRequest("No hay consultas con este criterio");
                
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //**POST Consultas/CrearRelacionConsultaCategoria
        [HttpPost("CrearRelacionConsultaCategoria")]
        public async Task<IActionResult> CrearRelacionConsultaCategoria([FromForm] Consulta nueva, [FromForm] String categoria){
            //primero corroborar que la consulta ya se encuentra en la base de datos
            var consulta = await contexto.Consultas.Where(consulta => consulta.Id == nueva.Id).FirstOrDefaultAsync();
            if(consulta == null){
                return BadRequest("La consulta no existe");
            }
            //obtener la categoria con todos los datos
            var categoriaParaRelacion = await contexto.Categorias.Where(categoriaBuscada => categoriaBuscada.Nombre == categoria).FirstOrDefaultAsync();
            if(categoriaParaRelacion == null){
                return BadRequest("La categoria no existe");
            }
            try{
                //crear una nueva relacion
                var nuevaRelacion = new Consulta_Categoria
                {
                    ConsultaId = nueva.Id,
                    Consulta = consulta,
                    CategoriaId = categoriaParaRelacion.Id,
                    Categoria = categoriaParaRelacion
                };
                //pegar en db
                var tareaasync = await contexto.Consultas_Categorias.AddAsync(nuevaRelacion);
                await contexto.SaveChangesAsync();
                //deveuelve la entidad
                return Ok(tareaasync.Entity);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
        ///** POST Consultas/ElegirRespuesta
        [HttpPost("ElegirRespuesta")]
        public async Task<IActionResult> ElegirRespuesta([FromBody] Respuesta respuesta){
            //primero obtener de la base de datos la consulta a la que pertenece esta respuesta para poder hacerle el tracking
            var consulta = await contexto.Consultas.Where(consulta => consulta.Id == respuesta.ConsultaId).FirstOrDefaultAsync();
            if(consulta == null){
                return BadRequest("La consulta no existe");
            }
            try{
                //corroborar si la consulta ya tiene una respuesta en caso de que tenga respuesta buscar dicha respuesta
                if(consulta.RespuestaSeleccionada != null){
                    var respuestaExistente = await contexto.Respuestas.Where(x => x.Id == consulta.RespuestaSeleccionada).FirstOrDefaultAsync();
                    if(respuestaExistente != null){
                        //obtener el usuario que realizo esta respuesta junto con su rating
                        var usuario = await contexto.Usuarios.Where(x => x.Id == respuestaExistente.UsuarioId).Include(x => x.Rating).FirstOrDefaultAsync();
                        //eliminar de su rating un punto a respuestas elegidas
                        usuario.Rating.RespuestasElegidas -= 1;
                    }
                }
                //asignar a la consulta la respuesta elegida
                consulta.RespuestaSeleccionada = respuesta.Id;
                consulta.Resuelto = true;
                //obtener el usuario que realizo la respuesta entrante
                var user = await contexto.Usuarios.Where(x => x.Id == respuesta.UsuarioId).Include(x => x.Rating).FirstOrDefaultAsync();
                //sumarle un punto a respuestas elegidas
                user.Rating.RespuestasElegidas += 1;
                //actualizar la base de datos
                await contexto.SaveChangesAsync();
                var respuestaAPI = new StringRespuesta();
                respuestaAPI.respuesta = "Respuesta seleccionada como la mas Util!";
                return Ok(respuestaAPI);
        
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
            
        }
    }
}
