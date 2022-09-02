using Microsoft.AspNetCore.Mvc;
using Emiliano_Chiapponi.Controllers.DTOS;

namespace Emiliano_Chiapponi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class VentaController : ControllerBase
    {
        // Probado desde Postman: (Devuelve las ventas que contienen productos cuyo IdUsuario = idUsuario en la tabla Producto y un "200 OK")
        // (GET "http://localhost:5232/Venta" Params KEY = idUsuario, VALUE = 2)
        // GET http://localhost:5232/Venta?idUsuario=2
        [HttpGet(Name = "TraerVentas")] // Se reciba el parámetro idUsuario desde la URL. El cuerpo de la petición siempre está vacío.
        public List<Venta> TraerVentas(long idUsuario)
        {
            return VentaHandler.TraerVentas(idUsuario);
        }



        // Probado desde Postman: OK (Devuelve TRUE y un "200 OK". Se valida que se agrego la Venta, los ProductoVendido y se actualizó el stock en Producto)
        // (POST "http://localhost:5232/Venta" Body raw JSON)
        /* [
            {
                "Id" : 1,
                "Descripcion" : "Producto1",
                "Costo" : 1,
                "PrecioVenta" : 1,
                "Stock" : 1,
                "IdUsuario" : 1
            },
            {
                "Id" : 2,
                "Descripcion" : "Producto2",
                "Costo" : 2,
                "PrecioVenta" : 2,
                "Stock" : 2,
                "IdUsuario" : 1
            }
        ]*/
        [HttpPost(Name = "CargarVenta")]    // No se reciben argumentos desde la URL. En el cuerpo de la petición está la lista de productos junto con el IdUsuario.
                                            // El nombre de los atributos, utilizado en el JSON (Postman), debe coincidir con el nombre de los atributos definidos en PostVenta.
                                            // Como se envía en el cuerpo de la petición una lista de Productos, en el método se recibe una List<PostVenta>
        public bool CargarVenta([FromBody] List<PostVenta> listaProductosVendidos)
        {
            long idVenta = 0;
            Venta venta = new Venta();
            venta.id = 1;

            idVenta = VentaHandler.CargarVenta(venta);
            if (idVenta >= 0)
            {
                List<ProductoVendido> productosVendidos = new List<ProductoVendido>();
                foreach (PostVenta item in listaProductosVendidos)
                {
                    ProductoVendido productoVendido = new ProductoVendido();
                    productoVendido.idProducto = item.Id;
                    productoVendido.stock = item.Stock;
                    productoVendido.idVenta = idVenta;

                    productosVendidos.Add(productoVendido);
                }

                if (ProductoVendidoHandler.CargarProductosVendidos(productosVendidos))
                {
                    Producto producto = new Producto();
                    bool resultado = false;

                    foreach (ProductoVendido item in productosVendidos)
                    {
                        producto.id = item.idProducto;
                        producto = ProductoHandler.ConsultarStock(producto);
                        //Console.WriteLine("El Stock del producto con Id = " + item.idProducto + " es: " + producto.stock);
                        producto.stock = producto.stock - item.stock;
                        resultado = ProductoHandler.ActualizarStock(producto);
                    }
                    return resultado;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
