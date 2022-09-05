using Microsoft.AspNetCore.Mvc;
using Emiliano_Chiapponi.Controllers.DTOS;

namespace Emiliano_Chiapponi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsuarioController : ControllerBase
    {
        //  GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   


        [HttpGet(Name = "TraerUsuario_conNombreUsuario")]
        public Usuario TraerUsuario_conNombreUsuario(string nombreUsuario)
        {
            return UsuarioHandler.TraerUsuario_conNombreUsuario(nombreUsuario);
        }


        /*[HttpGet("{nombreUsuario}/{contraseña}")]
        public Usuario InicioDeSesion(string nombreUsuario, string contraseña)
        {
            return UsuarioHandler.InicioDeSesion(nombreUsuario, contraseña);
        }*/


        //  PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   


        [HttpPut(Name = "ModificarUsuario")]
        public bool ModificarUsuario([FromBody] PutUsuario usuario)
        {
            try
            {
                return UsuarioHandler.ModificarUsuario(
                    new Usuario
                    {
                        Id = usuario.Id,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        NombreUsuario = usuario.NombreUsuario,
                        Contraseña = usuario.Contraseña,
                        Mail = usuario.Mail
                    }
                );
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        // POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   


        [HttpPost(Name = "CrearUsuario")]
        public bool CrearUsuario([FromBody] PostUsuario usuario)
        {
            try
            {
                return UsuarioHandler.CrearUsuario(
                    new Usuario
                    {
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        NombreUsuario = usuario.NombreUsuario,
                        Contraseña = usuario.Contraseña,
                        Mail = usuario.Mail
                    }
                );
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        // DELETE   DELETE    DELETE   DELETE    DELETE   DELETE    DELETE   DELETE    DELETE   DELETE    DELETE   DELETE    DELETE   DELETE    DELETE   DELETE    


        [HttpDelete(Name = "EliminarUsuario")]
        public bool EliminarUsuario([FromBody] long id)
        {
            try
            {
                return UsuarioHandler.EliminarUsuario(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}


