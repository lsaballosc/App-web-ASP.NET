using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{

//    IdVenta
//IdCliente
//TotalProducto
//MontoTotal
//Contacto
//IdDistrito
//Teléfono
//Dirección
//IdTransaccion
//FechaVenta
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }

        public int TotalProducto { get; set; }
        public decimal MontoTotal { get; set; }
        public string Contacto { get; set; }
        public String IdDistrito { get; set; }

        public string Telefono { get; set; }
        public string  Direccion{ get; set; }

        public string FechaTexto { get; set; }
        public string  IdTransaccion { get; set; }

        public List<DetalleVenta> oDetalleVenta { get; set; }
        public string ProvinciaDescripcion { get; set; }
        public string CantonDescripcion { get; set; }
        public string DistritoDescripcion { get; set; }
    }
}
