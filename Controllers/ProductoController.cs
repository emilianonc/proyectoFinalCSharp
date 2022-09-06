using Microsoft.AspNetCore.Mvc;
using Emiliano_Chiapponi.Controllers.DTOS;

namespace Emiliano_Chiapponi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductoController : ControllerBase
    {
        //  GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   


        /*// POSTMAN
        // Se envía:
        //      GET "http://localhost:5232/Producto" con Params KEY = idUsuario, VALUE = 2   ->   GET http://localhost:5232/Producto?idUsuario=2
        // Se recibe:
        //      los 2 productos en BD que tienen IdUsuario = 2 y un "200 OK"   ->   OK       
        [HttpGet] // Se recibe el parámetro idUsuario desde la URL. El cuerpo de la petición siempre está vacío.
        public List<Producto> TraerProductos_conIdUsuario(long idUsuario)
        {
            return ProductoHandler.TraerProductos_conIdUsuario(idUsuario);
        }*/


        // POSTMAN
        // Se envía:
        //      GET "http://localhost:5232/Producto" sin Params y sin Body   ->   GET http://localhost:5232/Producto
        // Se recibe:
        //      Todos los productos de la BD y un "200 OK"   ->   OK       
        [HttpGet(Name = "TraerProductos")]
        public List<Producto> TraerProductos()
        {
            return ProductoHandler.TraerProductos();
        }


        //  PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   PUT  PUT   


        // POSTMAN
        // Se envía:
        //      PUT "http://localhost:5232/Producto" Body raw JSON
        //      {
        //          "Id" : 4,
        //          "Descripcion" : "Musculosa_",
        //          "Costo" : 310,
        //          "PrecioVenta" : 1200,
        //          "Stock" : 30,
        //          "IdUsuario" :  1
        //      }
        // Se recibe:
        //      TRUE y un "200 OK". Se verifican los cambios de las columnas en BD   ->   OK

        // PUT
        // No se reciben argumentos desde la URL. En el cuerpo de la petición están los atributos del Producto.
        // El nombre de los atributos, utilizados en el JSON (Postman), debe coincidir con el nombre de los atributos definidos en PutProducto
        [HttpPut(Name = "ModificarProducto")]
                                              
        public bool ModificarProducto([FromBody] PutProducto producto)
        {
            try
            {
                return ProductoHandler.ModificarProducto(
                    new Producto
                    {
                        Id = producto.Id,
                        Descripciones = producto.Descripcion,
                        Costo = producto.Costo,
                        PrecioVenta = producto.PrecioVenta,
                        Stock = producto.Stock,
                        IdUsuario = producto.IdUsuario
                    }
                );
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //  POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   


        // POSTMAN
        // Se envía:
        //      POST "http://localhost:5232/Producto" Body raw JSON
        //      {
        //           "Descripcion" : "Bufanda",
        //           "Costo" : 150,
        //           "PrecioVenta" : 280,
        //           "Stock" : 50,
        //           "IdUsuario" :  2
        //      }
        // Se recibe: 
        //      TRUE y un "200 OK". Se valida que se agrego el producto en BD   ->   OK
        [HttpPost(Name = "CrearProducto")]  // No se reciben argumentos desde la URL. En el cuerpo de la petición están los atributos del Producto.
                                            // El nombre de los atributos, utilizado en el JSON (Postman), debe coincidir con el nombre de los atributos definidos en PostProducto.
        public bool CrearProducto([FromBody] PostProducto producto)
        {
            try
            {
                return ProductoHandler.CrearProducto(
                    new Producto
                    {
                        Descripciones = producto.Descripcion,
                        Costo = producto.Costo,
                        PrecioVenta = producto.PrecioVenta,
                        Stock = producto.Stock,
                        IdUsuario = producto.IdUsuario
                    }
                );
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //  DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   


        // POSTMAN 
        // Se envía:
        //      DELETE "http://localhost:5232/Producto" Body raw JSON
        //          13
        // Se recibe:
        //      TRUE y un "200 OK", se verifica que se elimina el producto de la BD y los ProductoVendido correspondientes.   ->   OK.
        [HttpDelete] // No se reciben argumentos desde la URL. El cuerpo de la petición contiene el argumento necesario.
                                   
        public bool EliminarProducto([FromBody] long idProducto)
        {
            try
            {
                ProductoVendidoHandler.EliminarProductoVendido(idProducto); // Puede que se quiera eliminar un producto que no fue vendido aún, por ello no se válida.
                return ProductoHandler.EliminarProducto(idProducto);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
