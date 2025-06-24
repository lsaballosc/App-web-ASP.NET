using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class DetalleVenta
    {
        //        IdDetalleVenta
        //IdVenta
        //IdProducto
        //Cantidad
        //Total

        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public Producto oProducto { get; set; }

        public int Cantidad { get; set; }
        public decimal Total { get; set; }

        public string IdTransaccion { get; set; }
    }
}
