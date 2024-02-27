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
    public class RatingsController : ControllerBase
        {
            private readonly DataContext contexto;
            private readonly IConfiguration config;

            public RatingsController(DataContext contexto, IConfiguration config )
            {
                this.contexto = contexto;
                this.config = config;
            }
            /**
            *Metodos necesarios:
            *       - obtener rating: Obtener(Rating rating)
            *       - crear rating: Crear(Rating rating)
            **/

            ////** GET: Ratings/Rating
            [HttpGet("Obtener")]
            public async Task<IActionResult> Obtener() {
                try{
                    var emailUsuario = User.Identity.Name;
                    var usuario = await contexto.Usuarios.Include(usuario => usuario.Rating).Where(usuario  => usuario.Email == emailUsuario).SingleOrDefaultAsync();
                    return usuario != null ? Ok(usuario.Rating) : BadRequest("Usuario no existe"); 
                }catch (Exception ex){
                    return BadRequest(ex.Message);
                }
            }

            ////** POST Ratings/Crear
            [HttpPost("Crear")]
            public async Task<IActionResult> Crear(){
                try{
                    var rating = new Rating();
                    var enviar = await contexto.Ratings.AddAsync(rating);
                    contexto.SaveChanges();
                    return Ok(enviar.Entity);
                }catch (Exception ex){
                    return BadRequest(ex.Message);
                }               
            }
        }
}