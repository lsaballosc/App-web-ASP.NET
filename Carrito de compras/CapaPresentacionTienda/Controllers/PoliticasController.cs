using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionTienda.Controllers
{
    public class PoliticasController : Controller
    {
        // GET: Politicas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Terminos()
        {
            return View();
        }
    }
}