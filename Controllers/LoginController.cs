using Microsoft.AspNetCore.Mvc;
using Emiliano_Chiapponi.Controllers.DTOS;

namespace Emiliano_Chiapponi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class LoginController : ControllerBase
    {
        // Probado desde Postman: (Devuelve todos los atributos del usuario con NombreUsuario = nombreUsuario y Contraseña = contraseña. En caso de que los datos no coincidan devuelve un usuario vacío. "200 OK")
        // (GET "http://localhost:5232/Login" Params KEY = nombreUsuario, VALUE = tcasazza, KEY = contraseña, VALUE = SoyTobiasCasazza)
        // GET http://localhost:5232/Login?nombreUsuario=tcasazza&contraseña=SoyTobiasCasazza
        [HttpGet(Name = "Login")] // Se reciben los parámetros nombreUsuario y contraseña desde la URL. El cuerpo de la petición siempre está vacío.
        public Usuario InicioDeSesion(string nombreUsuario, string contraseña)
        {
            return UsuarioHandler.InicioDeSesion(nombreUsuario, contraseña);
        }
    }
}
