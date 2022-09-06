using Microsoft.AspNetCore.Mvc;
using Emiliano_Chiapponi.Controllers.DTOS;

namespace Emiliano_Chiapponi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class VentaController : ControllerBase
    {
        //  GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   GET   


        [HttpGet(Name = "TraerVentas")]
        public List<ProductoVendido> TraerVentas()
        {
            return VentaHandler.TraerVentas();
        }


        //  POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   POST   


        // POSTMAN
        // Se envía:
        //      POST "http://localhost:5232/Venta" Body raw JSON
        //      [
        //         {
        //             "Id" : 1,
        //             "Descripcion" : "Producto1",
        //             "Costo" : 1,
        //             "PrecioVenta" : 1,
        //             "Stock" : 1,
        //             "IdUsuario" : 1
        //         },
        //         {
        //             "Id" : 2,
        //             "Descripcion" : "Producto2",
        //             "Costo" : 2,
        //             "PrecioVenta" : 2,
        //             "Stock" : 2,
        //             "IdUsuario" : 1
        //         }
        //      ]
        // Se recibe:
        //      TRUE y un "200 OK". Se valida que se agrego la Venta, se actualizó ProductoVendido y se actualizó el stock en Producto   ->   OK
        [HttpPost(Name = "CargarVenta")]    // No se reciben argumentos desde la URL. En el cuerpo de la petición está la lista de productos junto con el IdUsuario.
                                            // El nombre de los atributos, utilizado en el JSON (Postman), debe coincidir con el nombre de los atributos definidos en PostVenta.
                                            // Como se envía en el cuerpo de la petición una lista de Productos, en el método se recibe una List<PostVenta>
        public bool CargarVenta([FromBody] List<PostVenta> listaDeProductosVendidos)
        {
            // Validación:
            //  - Necesito Id y Stock de cada producto para poder actualizar las tablas Producto y ProductoVendido.
            //  - Necesito validar que el Id de Usuario sea válido (exista en base de datos)
            Producto producto = new Producto();
            Usuario usuario = new Usuario();
            foreach (PostVenta item in listaDeProductosVendidos)
            {
                producto = ProductoHandler.TraerProducto_conId(item.Id);
                if (producto.Id <= 0) // Verifico que todos los Id de Producto recibidos sean válidos
                {
                    return false;
                }
                
                if (item.Stock <= 0) // Verifico que las cantidades vendidas de cada producto sean mayores a cero
                {
                    return false; 
                }

                if (producto.Stock < item.Stock) // Verifico que el stock del producto sea suficiente para realizar la venta
                {
                    return false; 
                }

                usuario = UsuarioHandler.TraerUsuario_conId(item.IdUsuario);
                if (usuario.Id <= 0) // Verifico que el Id de Usuario asociado a la venta se encuentre en la BD
                {
                    return false;
                }
            }

            // Cargo una nueva venta en la tabla Venta
            Venta venta = new Venta();
            long idVenta = VentaHandler.CargarVenta(venta);
            // Si la Venta se cargó con exito continuo
            if (idVenta >= 0)
            {
                // Creo una lista de ProductoVendido a partir de "listaProductosVendidos"
                List<ProductoVendido> productosVendidos = new List<ProductoVendido>();
                foreach (PostVenta item in listaDeProductosVendidos)
                {
                    ProductoVendido productoVendido = new ProductoVendido();
                    productoVendido.IdProducto = item.Id;
                    productoVendido.Stock = item.Stock;
                    productoVendido.IdVenta = idVenta;
                    productosVendidos.Add(productoVendido);
                }
                // Cargo los productos vendidos en la tabla ProductoVendido
                if (ProductoVendidoHandler.CargarProductosVendidos(productosVendidos))
                {
                    // Si los productos vendidos se cargaron con exito continuo
                    bool resultado = false;

                    // Actualizo el stock de cada uno de los productos vendidos en la tabla Producto
                    foreach (ProductoVendido item in productosVendidos)
                    {
                        producto.Id = item.IdProducto;
                        producto = ProductoHandler.ConsultarStock(producto);
                        producto.Stock = producto.Stock - item.Stock;
                        resultado = ProductoHandler.ActualizarStock(producto);
                        if (resultado == false) // Si no se puede actualizar el stock de alguno de los productos rompo el bucle y devuelvo falso
                        {
                            break;
                        }
                    }
                    return resultado;
                }
                else
                {
                    return false; // Si no se pudieron cargar los productos vendidos en la tabla ProductoVendido devuelvo falso.
                }
            }
            else
            {
                return false; // Si no se pudo cargar la venta en la tabla Venta devuelvo falso.
            }
        }


        //  DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   DELETE   


        [HttpDelete(Name = "EliminarVenta")]
        public bool EliminarVenta([FromBody] long idVenta)
        {
            bool resultado = false;

            // Valido que el id recibido sea valido
            if (idVenta <= 0)
            {
                return false;
            }

            // Obtengo Id y stock de cada ProductoVendido correspondiente a la Venta
            List<ProductoVendido> productosVendidosDeLaVenta = new List<ProductoVendido>();
            productosVendidosDeLaVenta = ProductoVendidoHandler.TraerProductosVendidos_conIdVenta(idVenta);
            // Elimino los productos correspondientes de la tabla ProductoVendido
            if (ProductoVendidoHandler.EliminarProductoVendido_conIdVenta(idVenta))
            {
                // Si se pudieron eliminar los elementos de ProductoVendido, actualizo el stock
                Producto producto = new Producto();
                foreach (ProductoVendido item in productosVendidosDeLaVenta)
                {
                    producto.Id = item.IdProducto;
                    producto = ProductoHandler.ConsultarStock(producto);
                    producto.Stock = producto.Stock + item.Stock;
                    resultado = ProductoHandler.ActualizarStock(producto);
                    if (resultado == false) // Si no se puede actualizar el stock de alguno de los productos rompo el bucle y devuelvo falso
                    {
                        return false;
                    }
                }

                if (VentaHandler.EliminarVenta(idVenta))
                {
                    return true;
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
