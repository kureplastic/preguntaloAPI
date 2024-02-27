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
    public class CategoriasController : Controller
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public CategoriasController(DataContext contexto, IConfiguration config ){
            this.contexto = contexto;
            this.config = config;
        }
    //Metodos Necesarios
        //     - Crear Categoria
        //     - Obtener todas las Categorias

        //** GET: Categorias/Obtener
        [HttpGet("Obtener")]
        public async Task<IActionResult> Obtener() {
            try{
                var Categorias = await contexto.Categorias.ToListAsync();
                return Ok(Categorias);
            }catch (Exception ex){
                return BadRequest(ex.Message);
            }
        }
        //** POST Categorias/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Categoria nueva){
            try{
                var tareaasync = await contexto.Categorias.AddAsync(nueva);
                contexto.SaveChanges();
                return Ok(tareaasync.Entity);
            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}