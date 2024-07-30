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
            *       - CambiarPuntuacion(Puntuacion puntuacion)
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

            ////** GET: Ratings/Obtener/1
            [HttpGet("Obtener/{id}")]
            public async Task<IActionResult> Obtener(int id) {
                try{
                    var rating = await contexto.Ratings.Where(rating => rating.Id == id).SingleOrDefaultAsync();
                    return rating != null ? Ok(rating) : BadRequest("Rating no existe"); 
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

            ////** POST Ratings/CambiarPuntuacion
            [HttpPost("CambiarPuntuacion")]
            public async Task<IActionResult> CambiarPuntuacion([FromBody] Puntuacion puntuacion){
                Console.WriteLine("entro a Cambiar Puntuacion para consulta");
                //Primero tenemos que verificar si existe una puntuacion con el usuario logueado y consulta que viene dentro de Puntuacion
                try{
                    var emailUsuario = User.Identity.Name;
                    var puntacionExistente = await contexto.Puntuaciones.Where(x => x.ConsultaId == puntuacion.ConsultaId && x.UsuarioId == puntuacion.UsuarioId).SingleOrDefaultAsync();
                    if(puntacionExistente != null ){
                        if (puntacionExistente.revisarPuntuacion(puntuacion.PuntuacionPositiva)){
                            //aca hay que borrar la puntuacion existente
                            contexto.Puntuaciones.Remove(puntacionExistente);
                            //tambien hay que borrarle el valor a la consulta
                            var consulta = await contexto.Consultas.Where(x => x.Id == puntuacion.ConsultaId).SingleOrDefaultAsync();
                            consulta.EliminarPuntuacion(puntuacion.PuntuacionPositiva);
                            contexto.SaveChanges();
                            //debemos actualizar tambien rating del usuario
                            var result = await EliminarRatingPuntuacion(puntuacion,false);
                            if (result != null){
                                StringRespuesta respuesta = new StringRespuesta();
                                respuesta.respuesta = "Puntuacion eliminada";
                                return Ok(respuesta);
                            }
                            return BadRequest("Error al actualizar rating");
                        }else{
                            //actualizar la puntuacion existente
                            contexto.SaveChanges();
                        //debemos actualizar tambien rating del usuario esto implica eliminar el valor que ya estaba en la puntuacion Anterior
                        //y el valor dentro de puntuacion actualizarlo en el rating
                        var puntuacionAnterior = new Puntuacion
                        {
                            PuntuacionPositiva = !puntuacion.PuntuacionPositiva,
                            UsuarioId = puntuacion.UsuarioId,
                            ConsultaId = puntuacion.ConsultaId
                        };
                        //tambien debemos actualizar el valor de la consulta con puntuacion respectiva
                        var consulta = await contexto.Consultas.Where(x => x.Id == puntuacionAnterior.ConsultaId).SingleOrDefaultAsync();
                        puntuacionAnterior.Consulta = consulta;
                        consulta.EliminarPuntuacion(puntuacionAnterior.PuntuacionPositiva);
                        consulta.CambiarPuntuacion(puntuacion.PuntuacionPositiva);
                        contexto.SaveChanges();
                        //esto es para el rating del usuario
                        var result = await EliminarRatingPuntuacion(puntuacionAnterior,false);
                            var resul2 = await ActualizarRatingPuntuacion(puntuacion,false);
                            if (result != null && resul2 != null){
                                StringRespuesta respuesta = new StringRespuesta();
                                respuesta.respuesta = "Puntuacion actualizada";
                                return Ok(respuesta);
                            }
                            return BadRequest("Error al actualizar rating");
                        }   
                    }else{
                        //creamos la puntuacion en base de datos
                        var puntuacionNueva = new Puntuacion();
                        puntuacionNueva.PuntuacionPositiva = puntuacion.PuntuacionPositiva;
                        puntuacionNueva.ConsultaId = puntuacion.ConsultaId;
                        puntuacionNueva.UsuarioId = puntuacion.UsuarioId;
                        contexto.Puntuaciones.Add(puntuacionNueva);
                        //ahora tenemos que actualizar el valor de la consulta con puntuacion respectiva
                        var consulta = await contexto.Consultas.Where(x => x.Id == puntuacionNueva.ConsultaId).SingleOrDefaultAsync();
                        puntuacionNueva.Consulta = consulta;
                        consulta.CambiarPuntuacion(puntuacionNueva.PuntuacionPositiva);
                        contexto.SaveChanges();
                        //debemos actualizar tambien rating del usuario
                        var result = await ActualizarRatingPuntuacion(puntuacionNueva,false);
                        if (result != null){
                            StringRespuesta respuesta = new StringRespuesta();
                                respuesta.respuesta = "Puntuacion actualizada";
                                return Ok(respuesta);
                        }
                        return BadRequest("Error al actualizar rating");
                        
                    }
                }catch (Exception ex){
                    return BadRequest(ex.Message);
                }

            }

            ////** POST Ratings/CambiarPuntuacionRespuesta
            [HttpPost("CambiarPuntuacionRespuesta")]
            public async Task<IActionResult> CambiarPuntuacionRespuesta([FromBody] Puntuacion puntuacion){
                Console.WriteLine("entro a Cambiar Puntuacion para respuesta");
                //Primero tenemos que verificar si existe una puntuacion con el usuario logueado y consulta que viene dentro de Puntuacion
                try{
                    var emailUsuario = User.Identity.Name;
                    var puntacionExistente = await contexto.Puntuaciones.Where(x => x.RespuestaId == puntuacion.RespuestaId && x.UsuarioId == puntuacion.UsuarioId).SingleOrDefaultAsync();
                    if(puntacionExistente != null ){
                        if (puntacionExistente.revisarPuntuacion(puntuacion.PuntuacionPositiva)){
                            //aca hay que borrar la puntuacion existente
                            contexto.Puntuaciones.Remove(puntacionExistente);
                            //tambien hay que borrarle el valor a la respuesta
                            var respuesta = await contexto.Respuestas.Where(x => x.Id == puntuacion.RespuestaId).SingleOrDefaultAsync();
                            respuesta.EliminarPuntuacion(puntuacion.PuntuacionPositiva);
                            contexto.SaveChanges();
                            //debemos actualizar tambien rating del usuario
                            var result = await EliminarRatingPuntuacion(puntuacion,true);
                            if (result != null){
                                StringRespuesta respuestaAPI = new StringRespuesta();
                                respuestaAPI.respuesta = "Puntuacion eliminada";
                                return Ok(respuestaAPI);
                            }
                            return BadRequest("Error al actualizar rating");
                        }else{
                            //actualizar la puntuacion existente
                            contexto.SaveChanges();
                        //debemos actualizar tambien rating del usuario esto implica eliminar el valor que ya estaba en la puntuacion Anterior
                        //y el valor dentro de puntuacion actualizarlo en el rating
                        var puntuacionAnterior = new Puntuacion
                        {
                            PuntuacionPositiva = !puntuacion.PuntuacionPositiva,
                            UsuarioId = puntuacion.UsuarioId,
                            RespuestaId = puntuacion.RespuestaId
                        };
                        //tambien debemos actualizar el valor de la consulta con puntuacion respectiva
                        var respuesta = await contexto.Respuestas.Where(x => x.Id == puntuacionAnterior.RespuestaId).SingleOrDefaultAsync();
                        puntuacionAnterior.Respuesta = respuesta;
                        respuesta.EliminarPuntuacion(puntuacionAnterior.PuntuacionPositiva);
                        respuesta.CambiarPuntuacion(puntuacion.PuntuacionPositiva);
                        contexto.SaveChanges();
                        //esto es para el rating del usuario
                        var result = await EliminarRatingPuntuacion(puntuacionAnterior,true);
                            var resul2 = await ActualizarRatingPuntuacion(puntuacion,true);
                            if (result != null && resul2 != null){
                                StringRespuesta respuestaAPI = new StringRespuesta();
                                respuestaAPI.respuesta = "Puntuacion actualizada";
                                return Ok(respuestaAPI);
                            }
                            return BadRequest("Error al actualizar rating");
                        }   
                    }else{
                        //creamos la puntuacion en base de datos
                        var puntuacionNueva = new Puntuacion();
                        puntuacionNueva.PuntuacionPositiva = puntuacion.PuntuacionPositiva;
                        puntuacionNueva.RespuestaId = puntuacion.RespuestaId;
                        puntuacionNueva.UsuarioId = puntuacion.UsuarioId;
                        contexto.Puntuaciones.Add(puntuacionNueva);
                        //ahora tenemos que actualizar el valor de la consulta con puntuacion respectiva
                        var respuesta = await contexto.Respuestas.Where(x => x.Id == puntuacionNueva.RespuestaId).SingleOrDefaultAsync();
                        puntuacionNueva.Respuesta = respuesta;
                        respuesta.CambiarPuntuacion(puntuacionNueva.PuntuacionPositiva);
                        contexto.SaveChanges();
                        //debemos actualizar tambien rating del usuario
                        var result = await ActualizarRatingPuntuacion(puntuacionNueva,true);
                        if (result != null){
                            StringRespuesta respuestaAPI = new StringRespuesta();
                                respuestaAPI.respuesta = "Puntuacion actualizada";
                                return Ok(respuestaAPI);
                        }
                        return BadRequest("Error al actualizar rating");
                        
                    }
                }catch (Exception ex){
                    return BadRequest(ex.Message);
                }

            }

            ////** POST Ratings/ActualizarRatingPuntuacion
            [HttpPost("ActualizarRatingPuntuacion")]
            public async Task<IActionResult> ActualizarRatingPuntuacion(Puntuacion puntuacion, Boolean esRespuesta){
                //obtner el usuario el cual viene en puntuacion la consulta y de ahi buscar el rating
                // luego modificar rating con el metodo ModificarPuntuacion mas el valor que viene en puntuacion
                try{
                    Usuario usuario;
                    if (esRespuesta){
                        usuario = await contexto.Usuarios.Where(x => x.Id == puntuacion.Respuesta.UsuarioId).SingleOrDefaultAsync();
                    }
                    else{
                        usuario = await contexto.Usuarios.Where(x => x.Id == puntuacion.Consulta.UsuarioId).SingleOrDefaultAsync();
                    }
                    var rating = await contexto.Ratings.Where(x => x.Id == usuario.RatingId).SingleOrDefaultAsync();
                    rating.ModificarPuntuacion(puntuacion.PuntuacionPositiva);
                    contexto.SaveChanges();
                    return Ok("Rating actualizado");
                }catch (Exception ex){
                    return BadRequest(ex.Message);
                }
            }

            ////* POST Ratings/EliminarRatingPuntuacion
            [HttpPost("EliminarRatingPuntuacion")]
            public async Task<IActionResult> EliminarRatingPuntuacion(Puntuacion puntuacion, Boolean esRespuesta){
                //obtner el usuario el cual viene en puntuacion y de ahi buscar el rating
                // luego modificar rating con el metodo ModificarPuntuacion mas el valor que viene en puntuacion
                try{
                    Usuario usuario;
                    if (esRespuesta){
                        usuario = await contexto.Usuarios.Where(x => x.Id == puntuacion.Respuesta.UsuarioId).SingleOrDefaultAsync();
                    }else{
                        usuario = await contexto.Usuarios.Where (x => x.Id == puntuacion.Consulta.UsuarioId).SingleOrDefaultAsync();
                    }
                    var rating = await contexto.Ratings.Where(x => x.Id == usuario.RatingId).SingleOrDefaultAsync();
                    rating.EliminarPuntuacion(puntuacion.PuntuacionPositiva);
                    contexto.SaveChanges();
                    return Ok("Rating actualizado");
                }catch (Exception ex){
                    return BadRequest(ex.Message);
                }
            }
        }
}