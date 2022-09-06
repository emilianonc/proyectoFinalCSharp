using Microsoft.AspNetCore.Mvc;
using Emiliano_Chiapponi.Controllers.DTOS;

namespace Emiliano_Chiapponi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsuarioController : ControllerBase
    {
        //  GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   


        [HttpGet(Name = "TraerUsuario")]
        public Usuario TraerUsuario_conNombreUsuario(string nombreUsuario)
        {
            return UsuarioHandler.TraerUsuario_conNombreUsuario(nombreUsuario);
        }


        //  PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   PUT   


        [HttpPut(Name = "ModificarUsuario")]
        public bool ModificarUsuario([FromBody] PutUsuario usuario)
        {
            try
            {
                Usuario usuarioExistente = new Usuario();
                usuarioExistente = UsuarioHandler.TraerUsuario_conId(usuario.Id);
                if (usuarioExistente.Id <= 0)
                {
                    return false; // El Id de Usuario no existe en BD, no hay nada para modificar, debe crearse primero.
                }
                else
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


