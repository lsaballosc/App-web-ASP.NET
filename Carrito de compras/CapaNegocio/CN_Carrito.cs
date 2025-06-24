using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Carrito
    {


        private CD_Carrito objcdcarrito = new CD_Carrito();



        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            return objcdcarrito.ExisteCarrito(idcliente, idproducto);
        }

        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string mensaje)
        {
            return objcdcarrito.OperacionCarrito(idcliente,idproducto,sumar,out mensaje);
        }

        public int CantidadEnCarrito(int idcliente)
        {
            return objcdcarrito.CantidadEnCarrito(idcliente);
        }
        public List<Carrito> ListarProducto(int idcliente)
        {
            return objcdcarrito.ListarProducto(idcliente);
        }

        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            return objcdcarrito.EliminarCarrito(idcliente, idproducto);
        }
    }
}
