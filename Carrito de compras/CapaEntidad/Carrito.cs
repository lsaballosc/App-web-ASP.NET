﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrito
    {


        //        IdCarrito
        //IdCliente
        //IdProducto
        //Cantidad

        public int IdCarrito { get; set; }
        public Cliente oCliente { get; set; }
        public Producto oProducto { get; set; }

        public int Cantidad { get; set; }
    }
}
