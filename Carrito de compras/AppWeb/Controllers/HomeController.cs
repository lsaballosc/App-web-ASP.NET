using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AppWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        // retorno un JsonResult para que pueda ser consumido por el cliente
        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            // Creo un objeto de la capa de Entidad para acceder a los metodos de la capa de negocio
            List<Usuario> olista = new List<Usuario>();
            olista = new CN_Usuarios().Listar();

            // Retorno un JsonResult con la lista de usuarios
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public JsonResult GuardarUsuario(Usuario obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdUsuario == 0)
            {
                // Si el IdUsuario es 0, significa que es un nuevo usuario
                resultado = new CN_Usuarios().Registrar(obj, out mensaje);
            }
            else
            {
                // Si el IdUsuario es mayor a 0, significa que es un usuario existente
                resultado = new CN_Usuarios().Editar(obj, out mensaje);
            }

            // Retorno un JsonResult con el resultado de la operacion
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }






        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            // hago 2 variables para el resultado y el mensaje
            string mensaje = string.Empty;

            bool respuesta = false;

            respuesta = new CN_Usuarios().Eliminar(id, out mensaje);
            // Retorno un JsonResult con el resultado de la operacion
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }








        [HttpGet]
        public JsonResult ListaReporte(string fechainicio, string fechafin, string idtransaccion)
        {

            List<Reporte> olista = new List<Reporte>();



            // Retorno un JsonResult con la vista del dashboard
            olista = new CN_DashBoard().Ventas(fechainicio, fechafin, idtransaccion);



            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public JsonResult VistaDashboard()
        {
            // Retorno un JsonResult con la vista del dashboard
            CN_DashBoard oDashBoard = new CN_DashBoard();

            Dashboard result = oDashBoard.VerDashboard();

            return Json(new { resultado = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]



        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> olista = new CN_DashBoard().Ventas(fechainicio, fechafin, idtransaccion);

            var sb = new StringBuilder();

            // Encabezados
            sb.AppendLine("Fecha Venta;Cliente;Producto;Precio;Cantidad;Total;Id Transaccion");

            // Filas
            foreach (var rp in olista)
            {
                sb.AppendLine($"{rp.FechaVenta};{rp.Cliente};{rp.Producto};{rp.Precio};{rp.Cantidad};{rp.Total};{rp.IdTransaccion}");
            }

            // Codificación UTF-8 con BOM para Excel
            var utf8WithBom = new UTF8Encoding(true); // 'true' agrega el BOM
            byte[] buffer = utf8WithBom.GetBytes(sb.ToString());
            string fileName = $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            return File(buffer, "text/csv", fileName);
        }



    }
}