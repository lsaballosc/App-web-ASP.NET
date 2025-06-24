using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security; // Importar para usar FormsAuthentication

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Cliente obj)
        {

            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(obj.Nombres) ? "" : obj.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(obj.Apellidos) ? "" : obj.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(obj.Correo) ? "" : obj.Correo;

            if (obj.Clave != obj.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }



            resultado = new CN_Cliente().Registrar(obj, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }


        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {

            Cliente ocliente = null;

            ocliente = new CN_Cliente().Listar().Where(x => x.Correo == correo && x.Clave == CN_Recursos.EncriptarSHA256(clave)).FirstOrDefault();

            if (ocliente == null)
            {
                ViewBag.Error = "Correo o clave incorrectos.";
                return View();
            }
            else
            {
                if (ocliente.Reestablecer)
                {

                    TempData["IdCliente"] = ocliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(ocliente.Correo, false); // Iniciar sesión del Cliente
                    Session["Cliente"] = ocliente; // Guardar el cliente en la sesión

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda"); // Redirigir a la página de inicio
                }
            }

        }
        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {

            Cliente cliente = new Cliente();

            cliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if (cliente == null)
            {

                ViewBag.Error = "No se encontró un cliente relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(cliente.IdCliente, correo, out mensaje);
            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");


            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }

        [HttpPost]
        public ActionResult CambiarClave(string idcliente, string claveactual, string nuevaclave, string confirmarclave)
        {


            Cliente Cliente = new Cliente();
            Cliente = new CN_Cliente().Listar().Where(u => u.IdCliente == int.Parse(idcliente)).FirstOrDefault();

            if (Cliente.Clave != CN_Recursos.EncriptarSHA256(claveactual))
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La clave actual es incorrecta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            nuevaclave = CN_Recursos.EncriptarSHA256(nuevaclave);

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);



            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = mensaje;
                return View();
            }



        }
        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null; // Limpiar la sesión del cliente
            FormsAuthentication.SignOut();
            return RedirectToAction("Index"); // Redirigir a la página de inicio de sesión
        }
    }
}
