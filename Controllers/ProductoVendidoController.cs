using Microsoft.AspNetCore.Mvc;
using Emiliano_Chiapponi.Controllers.DTOS;

namespace Emiliano_Chiapponi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductoVendidoController : ControllerBase
    {
        //  GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   


        [HttpGet(Name = "TraerProductosVendidos_conIdUsuario")]
        public List<ProductoVendido> TraerProductosVendidos_conIdUsuario(long idUsuario)
        {
            return ProductoVendidoHandler.TraerProductosVendidos_conIdUsuario(idUsuario);
        }
    }
}
