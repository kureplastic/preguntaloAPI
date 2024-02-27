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
    public class RespuestasController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public RespuestasController(DataContext contexto, IConfiguration config )
            {
                this.contexto = contexto;
                this.config = config;
            }

        //Metodos Necesarios
        //     - Crear Respuesta
        //     - Obtener Respuesta
        //     - Editar Respuesta
        //     - Eliminar Respuesta
        //     - Obtener Respuestas de una consulta
        
        //* POST: Respuestas/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Respuesta nueva)
        {
            var ClaimId = ((ClaimsIdentity) User.Identity).FindFirst("UserId");
            Console.WriteLine("ClaimId: " + ClaimId.Value);
            //primero crear una nueva respuesta vacia
            Console.WriteLine("nueva.RespuestaId: " + nueva.RespuestaId);
            Console.WriteLine("nueva.ConsultaId: " + nueva.ConsultaId);
            if(nueva.RespuestaId == 0) {
                nueva.RespuestaId = null;
            }
            var nuevaRespuesta = new Respuesta
            {
                // llenar los campos necesarios
                Texto = nueva.Texto,
                ConsultaId = nueva.ConsultaId,
                UsuarioId = Int32.Parse(ClaimId.Value), //* Int32.Parse(ClaimId.Value) es la variable del usuario logueado, nueva.UsuarioId es para pruebas
                RespuestaId = nueva.RespuestaId
            };
            //pegar en db
            try
            {
                var tareaasync = await contexto.Respuestas.AddAsync(nuevaRespuesta);
                await contexto.SaveChangesAsync();
                //deveuelve la entidad
                return Ok(tareaasync.Entity);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //* GET Respuestas/Obtener/5
        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            try{
                var obtenida = await contexto.Respuestas.Where(respuesta => respuesta.Id == id).FirstOrDefaultAsync();
                if(obtenida == null){
                    return BadRequest("Respuesta no existe");
                }
                return Ok(obtenida);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //* PUT Respuestas/Editar
        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] Respuesta editada)
        {
            //obtener la respuesta original
            var original = contexto.Respuestas.Where(respuesta => respuesta.Id == editada.Id).FirstOrDefault();
            if(original == null){
                return BadRequest("Respuesta no encontrada");
            }
            //modificar los campos
            original.Texto = editada.Texto;
            //guarda en db
            try{
                await contexto.SaveChangesAsync();
                var modificado = contexto.Respuestas.Where(respuesta => respuesta.Id == editada.Id).FirstOrDefault();
                return Ok(modificado);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //* DELETE Respuestas/Eliminar/5
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = contexto.Respuestas.Where(respuesta => respuesta.Id == id).FirstOrDefault();
            if(respuesta == null){
                return BadRequest("Respuesta no encontrada");
            }
            try{
                contexto.Respuestas.Remove(respuesta);
                await contexto.SaveChangesAsync();
                return Ok("Respuesta eliminada");
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        //* GET Respuestas/ObtenerDeConsulta/5
        [HttpGet("ObtenerDeConsulta/{id}")]
        public async Task<IActionResult> ObtenerDeConsulta(int id)
        {
            try{
                var respuestas = await contexto.Respuestas.Where(respuesta => respuesta.ConsultaId == id).ToListAsync();
                if(respuestas == null){
                    return BadRequest("Error en respuestas");
                }
                return Ok(respuestas);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        // metodo que obtiene todas las respuestas de una respuesta en especifico
        //* GET Respuestas/ObtenerDeRespuesta/5
        [HttpGet("ObtenerDeRespuesta/{id}")]
        public async Task<IActionResult> ObtenerDeRespuesta(int id)
        {
            try{
                var respuestas = await contexto.Respuestas.Where(respuesta => respuesta.RespuestaId == id).ToListAsync();
                if(respuestas == null || respuestas.Count == 0){
                    return BadRequest("No se encontraron respuestas");
                }
                return Ok(respuestas);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
