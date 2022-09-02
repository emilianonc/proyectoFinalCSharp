using Microsoft.AspNetCore.Mvc;
using Emiliano_Chiapponi.Controllers.DTOS;

namespace Emiliano_Chiapponi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductoController : ControllerBase
    {
        // Probado desde Postman: (Devuelve los 2 productos en BD que tienen IdUsuario = 2 y un "200 OK")
        // (GET "http://localhost:5232/Producto" Params KEY = idUsuario, VALUE = 2)
        // GET http://localhost:5232/Producto?idUsuario=2
        [HttpGet(Name = "TraerProductosConIdUsuario")] // Se reciba el parámetro idUsuario desde la URL. El cuerpo de la petición siempre está vacío.
        public List<Producto> TraerProductosConIdUsuario(long idUsuario)
        {
            return ProductoHandler.TraerProductosConIdUsuario(idUsuario);
        }



        // Probado desde Postman: OK (Devuelve 7 y un "200 OK". Se valida que se agrego el producto en BD)
        // (POST "http://localhost:5232/Producto" Body raw JSON)
        /* {
                "Descripcion" : "Bufanda",
                "Costo" : 150,
                "PrecioVenta" : 280,
                "Stock" : 50,
                "IdUsuario" :  2
            }*/
        [HttpPost(Name = "CrearProducto")]  // No se reciben argumentos desde la URL. En el cuerpo de la petición están los atributos del Producto.
                                            // El nombre de los atributos, utilizado en el JSON (Postman), debe coincidir con el nombre de los atributos definidos en PostProducto.
        public long CrearProducto([FromBody] PostProducto producto)
        {
            try
            {
                return ProductoHandler.CrearProducto(
                    new Producto
                    {
                        descripcion = producto.Descripcion,
                        costo = producto.Costo,
                        precioVenta = producto.PrecioVenta,
                        stock = producto.Stock,
                        idUsuario = producto.IdUsuario
                    }
                );
            }
            catch (Exception ex)
            {
                return 0;
            }
        }



        // Probado desde Postman: OK (Devuelve TRUE y un "200 OK". Se verifican los cambios de las columnas en BD)
        // (PUT "http://localhost:5232/Producto" Body raw JSON)
        /* {
            "Id" : 4,
            "Descripcion" : "Musculosa_",   // Se agrega _
            "Costo" : 310,                  // Se sube 10
            "PrecioVenta" : 1200,           // Se sube 100
            "Stock" : 30,                   // Se sube 10
            "IdUsuario" :  1
        }*/
        [HttpPut(Name = "ModificarProducto")] // No se reciben argumentos desde la URL. En el cuerpo de la petición están los atributos del Producto.
                                              // El nombre de los atributos, utilizado en el JSON (Postman), debe coincidir con el nombre de los atributos definidos en PutProducto
        public bool ModificarProducto([FromBody] PutProducto producto)
        {
            try
            {
                return ProductoHandler.ModificarProducto(
                    new Producto
                    {
                        id = producto.Id,
                        descripcion = producto.Descripcion,
                        costo = producto.Costo,
                        precioVenta = producto.PrecioVenta,
                        stock = producto.Stock,
                        idUsuario = producto.IdUsuario
                    }
                );
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        // Probado desde Postman: (). Se verifica que se elimina el producto de la BD y los ProductoVendido correspondientes.
        // (DELETE "http://localhost:5232/Producto" Body raw JSON)
        /* {
               "Id" : 6
           }*/
        [HttpDelete(Name = "EliminarProducto")] // No se reciben argumentos desde la URL. En el cuerpo de la petición está el Id del Producto a eliminar.
                                                // El nombre de los atributos, utilizado en el JSON (Postman), debe coincidir con el nombre de los atributos definidos en DeleteProducto
        public bool EliminarProducto([FromBody] DeleteProducto producto)
        {
            try
            {
                if (ProductoVendidoHandler.EliminarProductoVendido(producto.Id))
                {
                    return ProductoHandler.EliminarProducto(producto.Id);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
