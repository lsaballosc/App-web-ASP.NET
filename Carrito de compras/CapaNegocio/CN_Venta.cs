using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CapaNegocio
{
    public class CN_Venta
    {

        CD_Venta objventa = new CD_Venta();
        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            return objventa.Registrar(obj, DetalleVenta,out Mensaje);
        }
    }
}
