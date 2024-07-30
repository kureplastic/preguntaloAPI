using preguntaloAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using NuGet.Protocol;


namespace preguntaloAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
        {
            private readonly DataContext contexto;
            private readonly IConfiguration config;

            public UsuariosController(DataContext contexto, IConfiguration config )
            {
                this.contexto = contexto;
                this.config = config;
            }
            /**
            *Metodos necesarios:
            *       - registrar usuario: Registrar(Usuario usuario)
            *       - editar usuario: Editar(Usuario usuario)
            *       - obtener Perfil: Perfil()
            *       - login de usuario: Login()
            **/


            ////** GET: Usuarios/Perfil
            [HttpGet("Perfil")]
            public async Task<IActionResult> Perfil() {
                try{
                    var ClaimID = ((ClaimsIdentity) User.Identity).FindFirst("UserId");
                    var entidad = await contexto.Usuarios.SingleOrDefaultAsync(usuario => usuario.Id == Int32.Parse(ClaimID.Value));
                    if (entidad != null){
                        //obtengamos el perfil junto con el rating y la validacion si existe
                        var usuarioCompleto = await contexto.Usuarios.Include(u => u.Validacion).Where(u => u.Id == entidad.Id).SingleOrDefaultAsync();
                        return usuarioCompleto != null ? Ok(usuarioCompleto) : BadRequest("Entidad no existe");
                    }
                    return BadRequest("Entidad no existe");
                }
                catch (Exception ex){
                    return BadRequest(ex.Message);
                }
            }

            ////** POST Usuarios/Registrar
            [HttpPost("Registrar")]
            [AllowAnonymous]
            public async Task<ActionResult> RegistrarUsuario([FromBody] Usuario nuevo) 
            {
                //primero buscar mail que ingresa para corrobar que no existe
                try
                {
                    var entidad = await contexto.Usuarios.SingleOrDefaultAsync(usuario => usuario.Email == nuevo.Email);
                    //si no existe registrar usuario
                    if(entidad == null){
                        var registro = new Usuario();
                        registro.Apellido = nuevo.Apellido;
                        registro.Nombre = nuevo.Nombre;
                        registro.DNI = nuevo.DNI;
                        registro.Email = nuevo.Email;
                        registro.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: nuevo.Password,
							salt: System.Text.Encoding.ASCII.GetBytes(config["SALT"]),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));

                        registro.FotoPerfil = null;
                        registro.ValidacionId = null;
                        //para este registro se deberian crear en el momento un rating nuevo
                        try{
                            var rating = new Rating();
                            var tareaasyncRating = await contexto.Ratings.AddAsync(rating);
                            await contexto.SaveChangesAsync();
                            if(tareaasyncRating == null){
                                return BadRequest("Error al registrar rating");
                            }else{
                                registro.RatingId = rating.Id;
                                try{
                                    Console.WriteLine("entro correctamente al final de la ejecucion");
                                    var tareaasync = await contexto.Usuarios.AddAsync(registro);
                                    await contexto.SaveChangesAsync();
                                    //deveuelve la entidad
                                    return Ok(tareaasync.Entity);
                                }catch(Exception ex){
                                    Console.WriteLine(ex.Message);
                                    return BadRequest(ex.Message);
                                }
                            }

                        }catch(Exception ex){
                            Console.WriteLine(ex.Message);
                            return BadRequest(ex.Message);
                        }
                        
                    }else{
                        return BadRequest("Ya existe un usuario con ese email!"); 
                    } 
                }catch (Exception ex){
                    Console.WriteLine("problema general de la ejecucion");
                    return BadRequest(ex.Message);
                }
                //loguear usuario
                //acceder al sistema
            }

            ////** PUT Usuarios/Editar
            [HttpPut("Editar")]
            public async Task<ActionResult> Editar([FromBody] Usuario obtenido)
            {
                //primero buscar por mail
                try
                {
                    var entidad = contexto.Usuarios.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                    //Luego de obtener la entidad modificar lo que se desee.
                    if(entidad != null)
                    {
                        //modifica password
                        //*hacer un metodo diferente
                        //modifica otros campos
                        entidad.Apellido = obtenido.Apellido;
                        entidad.Nombre = obtenido.Nombre;
                        entidad.DNI = obtenido.DNI;
                        try
                        {
                            await contexto.SaveChangesAsync();
                            var modificado = contexto.Usuarios.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
					        return Ok(modificado);
                        } catch(Exception ex) {
                            Console.WriteLine("Error en SaveChanges: " + ex.Message);
                            return BadRequest("Error en SaveChanges: " + ex.Message);
                        }   
                    }else { 
                        Console.WriteLine("Entidad no encontrada");
                        return BadRequest("Entidad no encontrada"); 
                    } 
                } catch (Exception ex) { 
                    Console.WriteLine("Error general "+ ex.Message);
                    return BadRequest("Error general "+ ex.Message); 
                }
            }

            ////** PUT Usuarios/EditarPassword
            [HttpPut("EditarPassword")]
            public async Task<ActionResult> EditarPassword([FromBody] PassView passview)
            {
                //primero buscar por mail
                try
                {
                    var entidad = contexto.Usuarios.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                    if(entidad != null)
                    {
                        //modifica password
                        //*primero corroborar que el password viejo sea el correcto
                        try{
                            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: passview.Password,
					            salt: System.Text.Encoding.ASCII.GetBytes(config["SALT"]),
					            prf: KeyDerivationPrf.HMACSHA1,
					            iterationCount: 1000,
					            numBytesRequested: 256 / 8));
                            if(hashed == entidad.Password){
                                //? deberia controlar del lado del back si newPass y ConfirmPass son iguales?
                                //* modifica password con newPassword
                                entidad.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                    password: passview.NewPassword,
                                    salt: System.Text.Encoding.ASCII.GetBytes(config["SALT"]),
                                    prf: KeyDerivationPrf.HMACSHA1,
                                    iterationCount: 1000,
                                    numBytesRequested: 256 / 8));
                            }else{
                                return BadRequest("Password incorrecto");
                            }
                        }catch(Exception ex){
                            return BadRequest("Error en cambiar pass: " + ex.Message);
                        }
                        try
                        {
                            await contexto.SaveChangesAsync();
                            var modificado = contexto.Usuarios.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
					        return Ok(modificado);
                        } catch(Exception ex) {
                            return BadRequest("Error en SaveChanges: " + ex.Message);
                        }   
                    }else { 
                        return BadRequest("Entidad no encontrada"); 
                    } 
                } catch (Exception ex) { 
                    return BadRequest("Error general "+ ex.Message); 
                }
            }

            //** PUT Usuarios/EditarFotoPerfil
            [HttpPut("EditarFotoPerfil")]
            public async Task<ActionResult> EditarFotoPerfil([FromBody] string url)
            {
                var entidad = contexto.Usuarios.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                if(entidad != null){
                    entidad.FotoPerfil = url;
                    try{
                        await contexto.SaveChangesAsync();
                        var modificado = contexto.Usuarios.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                        return Ok(modificado);
                    }catch(Exception ex){
                        return BadRequest(ex.Message);
                    }
                }else{
                    return BadRequest("Entidad no encontrada");
                }
            }

            //** POST Usuarios/login
            [HttpPost("login")]
            [AllowAnonymous]
            public async Task<IActionResult> Login([FromBody] LoginView loginView)
            {
                try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginView.Password,
					salt: System.Text.Encoding.ASCII.GetBytes(config["SALT"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
				var usuario = await contexto.Usuarios.FirstOrDefaultAsync(x => x.Email == loginView.Email);
                Console.WriteLine("Clave: " + loginView.Password + " Hash: " + hashed);
                Console.WriteLine("Usuario: " + usuario.Email);
				if (usuario == null || usuario.Password != hashed) //testear con |loginView.Clave != "prueba"|   en vez de   | usuario.Password != hashed |
				{
					Console.WriteLine("no encuentra Usuario");
                    return BadRequest("Nombre de usuario o clave incorrecta");
				}
				else
				{
                    Console.WriteLine("encontro al usuario y lo loguea");
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, usuario.Email),
                        new Claim("UserId", usuario.Id.ToString()),
					};

					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddDays(30),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}    
            }

            //** POST Usuarios/CrearValidacion
            [HttpPost("CrearValidacion")]
            public async Task<IActionResult> CrearValidacion([FromBody] Validacion validacion){
                //obtener usuario logueado
                var emailUsuario = User.Identity.Name;
                try{
                    var usuario = await contexto.Usuarios.Where(x => x.Email == emailUsuario).FirstOrDefaultAsync();
                    if(usuario.ValidacionId != null){
                        return BadRequest("Ya existe una validacion");
                    }
                    validacion.Confirmada = true;
                    var enviar = await contexto.Validaciones.AddAsync(validacion);
                    if(enviar != null){
                        var result = await contexto.SaveChangesAsync();
                        usuario.ValidacionId = enviar.Entity.Id;
                        contexto.SaveChanges();
                        return Ok(enviar.Entity);
                    }else{
                        return BadRequest("Error al crear validacion");
                    }
                }catch (Exception ex){
                    return BadRequest(ex.Message);
                }
            }
            
        }
}