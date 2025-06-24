using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace AppWeb.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario usuario = new Usuario();
            usuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.EncriptarSHA256(clave)).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña inválidos";
                return View();
            }
            else
            {

                if (usuario.Reestablecer)
                {
                    TempData["IdUsuario"] = usuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }
                FormsAuthentication.SetAuthCookie(usuario.Correo, false);

                Session["NombreUsuario"] = usuario.Nombres;
                Session["Apellidos"] = usuario.Apellidos;
                Session["CorreoUsuario"] = usuario.Correo;


                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }




        }
        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)

        {

            Usuario usuario = new Usuario();
            usuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();

            if (usuario.Clave != CN_Recursos.EncriptarSHA256(claveactual))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La clave actual es incorrecta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            nuevaclave = CN_Recursos.EncriptarSHA256(nuevaclave);

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);



            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = idusuario;
                ViewBag.Error = mensaje;
                return View();
            }

        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario ousuario = new Usuario();

            ousuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if (ousuario == null)
            {

                ViewBag.Error = "No se encontró un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().ReestablecerClave(ousuario.IdUsuario, correo, out mensaje);
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


        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut(); 
            return RedirectToAction("Index"); // Redirigir a la página de inicio de sesión
        }
    }
}