using Microsoft.AspNetCore.Mvc;
using Emiliano_Chiapponi.Controllers.DTOS;

namespace Emiliano_Chiapponi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductoVendidoController : ControllerBase
    {
        // Probado desde Postman:  (Devuelve: lista de objetos ProductoVendido que tienen IdUsuario = idUsuario en la tabla producto y un "200 OK" )
        // (GET "http://localhost:5232/ProductoVendido" Params KEY = idUsuario, VALUE = 2)
        // GET http://localhost:5232/ProductoVendido?idUsuario=2
        [HttpGet(Name = "TraerProductosVendidos")] // Se recibe el parámetro idUsuario desde la URL. El cuerpo de la petición siempre está vacío.
        public List<ProductoVendido> TraerProductosVendidos(long idUsuario)
        {
            return ProductoVendidoHandler.TraerProductosVendidos(idUsuario);
        }
    }
}
