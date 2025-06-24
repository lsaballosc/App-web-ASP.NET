using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace CapaPresentacionTienda.Filter
{
    // clase heredad
    public class ValidarSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Verificar si la sesión está activa
            if (HttpContext.Current.Session["Cliente"] == null)
            {
                // Redirigir a la página de inicio de sesión si no hay sesión activa
                filterContext.Result = new RedirectResult("~/Acceso/Index");
                return;
            }
            else
            {
                // Si la sesión está activa, continuar con la acción
                base.OnActionExecuting(filterContext);
            }
        }
    }
}