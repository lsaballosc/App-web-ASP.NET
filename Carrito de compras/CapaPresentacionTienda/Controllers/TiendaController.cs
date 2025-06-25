using CapaEntidad;
using CapaEntidad.PayPal;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using CapaPresentacionTienda.Filter;




namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DetalleProducto(int idproducto = 0)
        {
            Producto producto = new Producto();

            bool conversion;

            producto = new CN_Producto().Listar().Where(p => p.IdProducto == idproducto).FirstOrDefault();
            if (producto != null)
            {
                producto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(producto.RutaImagen, producto.NombreImagen), out conversion);
                producto.Extension = Path.GetExtension(producto.NombreImagen);
            }

            return View(producto);
        }

        public ActionResult About()
        {
            return View();
        }


        public ActionResult Instrucciones()
        {
            return View();
        }

        // devuelve la lista de categorias

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().Listar();


            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ListarMarcaPorCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();

            lista = new CN_Marca().ListarMarcaporCategoria(idcategoria);


            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca, int nroPagina)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            //lista = new CN_Producto().Listar().Select(p => new Producto()
            //{
            //    IdProducto = p.IdProducto,
            //    Nombre = p.Nombre,
            //    Descripcion = p.Descripcion,
            //    oMarca = p.oMarca,
            //    oCategoria = p.oCategoria,
            //    Precio = p.Precio,
            //    Stock = p.Stock,
            //    RutaImagen = p.RutaImagen,
            //    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
            //    Extension = Path.GetExtension(p.NombreImagen),
            //    Activo = p.Activo



            //}).Where(p =>
            //    p.oCategoria.idCategoria == (idcategoria == 0 ? p.oCategoria.idCategoria : idcategoria) &&
            //    p.oMarca.IdMarca == (idmarca == 0 ? p.oMarca.IdMarca : idmarca) &&
            //    p.Stock > 0 && p.Activo == true




            //).ToList();

            int _totalRegistros;
            int _totalPaginas;
            lista = new CN_Producto().ObtenerProductos(idmarca,idcategoria, nroPagina, 8, out _totalRegistros, out _totalPaginas).Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo



            }).ToList();









            var jsonresult = Json(new { data = lista, totalRegistros = _totalRegistros, totalPaginas = _totalPaginas }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }






        //agregar carrito

        [HttpPost]

        public JsonResult AgregarCarrito(int idproducto)
        {
            // lo convierto a cliente
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);

            bool respuesta = false;

            string mensaje = String.Empty;

            if (existe)
            {
                mensaje = "El Producto ya existe en el carrito";
            }
            else
            {
                // TRUE PORQUE VA A AGREGAR AL CARRO
                respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);

            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }// Fin del método





        [HttpPost]

        public JsonResult ListarProductosCarrito()
        {

            try
            {
                int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

                List<Carrito> lista = new List<Carrito>();

                bool conversion;

                lista = new CN_Carrito().ListarProducto(idcliente).Select(oc => new Carrito()
                {
                    oProducto = new Producto()
                    {
                        IdProducto = oc.oProducto.IdProducto,
                        Nombre = oc.oProducto.Nombre,
                        oMarca = oc.oProducto.oMarca,
                        Precio = oc.oProducto.Precio,
                        RutaImagen = oc.oProducto.RutaImagen,
                        Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                        Extension = Path.GetExtension(oc.oProducto.NombreImagen)

                    },
                    Cantidad = oc.Cantidad




                }).ToList();
                return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message });
            }

        }// fin del metodo


        // este métod permite hacer las operaciones dentro del carrito para agregar o disminuir
        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            // lo convierto a cliente
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;



            bool respuesta = false;

            string mensaje = String.Empty;
            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }// fin

        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;

            string mensaje = String.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }// fin


        [HttpPost]
        public JsonResult ObtenerProvincia()
        {
            List<Provincia> olista = new List<Provincia>();
            olista = new CN_Ubicacion().ObtenerProvincia();

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerCanton(string idprovincia)
        {
            List<Canton> olista = new List<Canton>();
            olista = new CN_Ubicacion().ObtenerCanton(idprovincia);

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ObtenerDistrito(string idprovincia, string idcanton)
        {
            List<Distrito> olista = new List<Distrito>();
            olista = new CN_Ubicacion().ObtenerDistrito(idprovincia, idcanton);

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }

        [ValidarSession]
      
        public ActionResult Carrito()
        {
            return View();
        }


        [HttpGet]
        private async Task<decimal> ObtenerTipoCambioCRCtoUSD()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync("https://open.er-api.com/v6/latest/CRC");
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json);

                    decimal tipoCambio = (decimal)result.rates.USD;

                    return tipoCambio;
                }
                catch
                {
                    return 545m; // Valor por defecto si falla la API
                }
            }
        }


        //[HttpPost]

        //public async Task<JsonResult> ProcesarPago(List<Carrito> olistaCarrito, Venta oVenta)
        //{
        //    // Obtener tipo de cambio CRC a USD desde la API externa
        //    decimal tipoCambio = await ObtenerTipoCambioCRCtoUSD();

        //    decimal totalCRC = 0;
        //    decimal totalUSD = 0;

        //    DataTable detalle_venta = new DataTable();
        //    detalle_venta.Locale = new CultureInfo("en-CR");
        //    detalle_venta.Columns.Add("IdProducto", typeof(string));
        //    detalle_venta.Columns.Add("Cantidad", typeof(int));
        //    detalle_venta.Columns.Add("Total", typeof(decimal));

        //    List<Item> oListaItem = new List<Item>();

        //    foreach (Carrito oCarrito in olistaCarrito)
        //    {
        //        decimal subtotalCRC = oCarrito.Cantidad * oCarrito.oProducto.Precio;
        //        totalCRC += subtotalCRC;

        //        // Convertir precio unitario a USD (multiplicando, porque 1 CRC = 0.00183 USD aprox.)
        //        decimal precioUnitarioUSD = oCarrito.oProducto.Precio * tipoCambio;
        //        precioUnitarioUSD = Math.Round(precioUnitarioUSD, 2);

        //        // Calcular subtotal del producto en USD y sumarlo al totalUSD
        //        decimal subtotalUSD = precioUnitarioUSD * oCarrito.Cantidad;
        //        totalUSD += subtotalUSD;

        //        // Agregar ítem para PayPal
        //        oListaItem.Add(new Item()
        //        {
        //            name = oCarrito.oProducto.Nombre,
        //            quantity = oCarrito.Cantidad.ToString(),
        //            unit_amount = new UnitAmount()
        //            {
        //                currency_code = "USD",
        //                value = precioUnitarioUSD.ToString("F2", CultureInfo.InvariantCulture)
        //            }
        //        });

        //        detalle_venta.Rows.Add(new object[]
        //        {
        // oCarrito.oProducto.IdProducto,
        // oCarrito.Cantidad,
        // subtotalCRC
        //        });
        //    }

        //    // Redondear totalUSD a 2 decimales para cumplir con PayPal
        //    totalUSD = Math.Round(totalUSD, 2);

        //    PurchaseUnit purchaseUnit = new PurchaseUnit()
        //    {
        //        amount = new Amount()
        //        {
        //            currency_code = "USD",
        //            value = totalUSD.ToString("F2", CultureInfo.InvariantCulture),
        //            breakdown = new Breakdown()
        //            {
        //                item_total = new ItemTotal()
        //                {
        //                    currency_code = "USD",
        //                    value = totalUSD.ToString("F2", CultureInfo.InvariantCulture)
        //                }
        //            }
        //        },
        //        description = "Compra de Productos en la tienda",
        //        items = oListaItem
        //    };

        //    Checkout_Order oCheckOutOrder = new Checkout_Order()
        //    {
        //        intent = "CAPTURE",
        //        purchase_units = new List<PurchaseUnit>() { purchaseUnit },
        //        application_context = new ApplicationContext()
        //        {
        //            brand_name = "JzelCrochet.com",
        //            landing_page = "NO_PREFERENCE",
        //            user_action = "PAY_NOW",
        //            return_url = "https://localhost:44374/Tienda/PagoEfectuado",
        //            cancel_url = "https://localhost:44374/Tienda/Carrito"
        //        }
        //    };

        //    oVenta.MontoTotal = totalCRC; // Guardado en colones
        //    oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

        //    TempData["Venta"] = oVenta;
        //    TempData["DetalleVenta"] = detalle_venta;

        //    CN_Paypal objPaypal = new CN_Paypal();
        //    Response_Paypal<Response_Checkout> response = await objPaypal.CrearSolicitud(oCheckOutOrder);

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> olistaCarrito, Venta oVenta)
        {
            // Obtener tipo de cambio CRC a USD desde la API externa
            decimal tipoCambio = await ObtenerTipoCambioCRCtoUSD();

            decimal totalCRC = 0;
            decimal totalUSD = 0;

            // DataTable para guardar el detalle que irá a la base de datos
            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("en-CR");
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            // Lista de items para PayPal
            List<Item> oListaItem = new List<Item>();

            // Lista de detalle para asignar al objeto Venta
            List<DetalleVenta> listaDetalleVenta = new List<DetalleVenta>();

            foreach (Carrito oCarrito in olistaCarrito)
            {
                decimal subtotalCRC = oCarrito.Cantidad * oCarrito.oProducto.Precio;
                totalCRC += subtotalCRC;

                // Convertir precio unitario a USD
                decimal precioUnitarioUSD = oCarrito.oProducto.Precio * tipoCambio;
                precioUnitarioUSD = Math.Round(precioUnitarioUSD, 2);

                decimal subtotalUSD = precioUnitarioUSD * oCarrito.Cantidad;
                totalUSD += subtotalUSD;

                // Agregar ítem para PayPal
                oListaItem.Add(new Item()
                {
                    name = oCarrito.oProducto.Nombre,
                    quantity = oCarrito.Cantidad.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "USD",
                        value = precioUnitarioUSD.ToString("F2", CultureInfo.InvariantCulture)
                    }
                });

                // Agregar fila al DataTable (para la base de datos)
                detalle_venta.Rows.Add(new object[]
                {
            oCarrito.oProducto.IdProducto,
            oCarrito.Cantidad,
            subtotalCRC
                });

                // Agregar al detalle de la venta (para el objeto Venta completo)
                listaDetalleVenta.Add(new DetalleVenta
                {
                    oProducto = oCarrito.oProducto,
                    Cantidad = oCarrito.Cantidad,
                    Total = subtotalCRC
                });
            }

            totalUSD = Math.Round(totalUSD, 2);

            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = totalUSD.ToString("F2", CultureInfo.InvariantCulture),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "USD",
                            value = totalUSD.ToString("F2", CultureInfo.InvariantCulture)
                        }
                    }
                },
                description = "Compra de Productos en la tienda",
                items = oListaItem
            };

            Checkout_Order oCheckOutOrder = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>() { purchaseUnit },
                application_context = new ApplicationContext()
                {
                    brand_name = "JzelCrochet.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44374/Tienda/PagoEfectuado",
                    cancel_url = "https://localhost:44374/Tienda/Carrito"
                }
            };

            // Asignamos el total y el detalle al objeto Venta
            oVenta.MontoTotal = totalCRC; // En colones
            oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;
            oVenta.TotalProducto = listaDetalleVenta.Sum(x => x.Cantidad);
            oVenta.oDetalleVenta = listaDetalleVenta;

            // Guardamos en TempData para usarlo después
            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            // Creamos la orden de PayPal
            CN_Paypal objPaypal = new CN_Paypal();
            Response_Paypal<Response_Checkout> response = await objPaypal.CrearSolicitud(oCheckOutOrder);

            return Json(response, JsonRequestBehavior.AllowGet);
        }






        //public async Task<ActionResult> PagoEfectuado()
        //{
        //    string token = Request.QueryString["token"];
        //  CN_Paypal objPaypal = new CN_Paypal();

        //    Response_Paypal<Response_Capture> response = await objPaypal.AprobarPago(token);

        //   ViewData["Status"] = response.Status;

        //    if (response.Status)
        // {
        //     Venta oVenta = (Venta)TempData["Venta"];

        //        DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

        //        oVenta.IdTransaccion = response.Response.purchase_units[0].payments.captures[0].id;

        //        string mensaje = string.Empty;
        //       bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

        //        if (respuesta)
        //        {
        //            Cliente cliente = (Cliente)Session["Cliente"];
        //            CN_Recursos.EnviarFacturaCorreo(cliente.Correo, cliente.Nombres, oVenta);
        //        }

        //        ViewData["IdTransaccion"] = oVenta.IdTransaccion;
        //    }
        //    return View();



        //}
        [ValidarSession]
      

        public async Task<ActionResult> PagoEfectuado()
        {
            string token = Request.QueryString["token"];
            CN_Paypal objPaypal = new CN_Paypal();

            Response_Paypal<Response_Capture> response = await objPaypal.AprobarPago(token);
            ViewData["Status"] = response.Status;

            if (response.Status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];
                oVenta.IdTransaccion = response.Response.purchase_units[0].payments.captures[0].id;

                string mensaje = string.Empty;
                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

                if (respuesta)
                {
                    //  obtener datos de provincia, canton y distrito usando IdDistrito
                    Distrito datosUbicacion = new CN_Ubicacion().ObtenerDatosUbicacionPorDistrito(oVenta.IdDistrito);

                    oVenta.ProvinciaDescripcion = datosUbicacion?.ProvinciaDescripcion ?? "";
                    oVenta.CantonDescripcion = datosUbicacion?.CantonDescripcion ?? "";
                    oVenta.DistritoDescripcion = datosUbicacion?.Descripcion ?? "";

                    // El correo solo necesita el correo y nombre del cliente (para el saludo)
                    Cliente cliente = (Cliente)Session["Cliente"];
                    CN_Recursos.EnviarFacturaCorreo(cliente.Correo, cliente.Nombres, oVenta);
                }

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;
            }
            return View();
        }



        // metodo historial de compras

        [ValidarSession]
       
        public ActionResult MisCompras()
        {

            try
            {
                int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

                List<DetalleVenta> lista = new List<DetalleVenta>();

                bool conversion;

                lista = new CN_Venta().ListarCompras(idcliente).Select(oc => new DetalleVenta()
                {
                    oProducto = new Producto()
                    {
                    
                        Nombre = oc.oProducto.Nombre,
                       
                        Precio = oc.oProducto.Precio,
                     
                        Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                        Extension = Path.GetExtension(oc.oProducto.NombreImagen)

                    },
                    Cantidad = oc.Cantidad,
                    Total = oc.Total,
                    IdTransaccion = oc.IdTransaccion




                }).ToList();
                return View(lista);
            }
            catch (Exception ex)
            {
               return View(new List<DetalleVenta>()); // Retorna una lista vacía en caso de error
            }

        }// fin del metodo




    }
}





