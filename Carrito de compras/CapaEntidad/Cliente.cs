using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    //    IdCliente
    //Nombres
    //Apellidos
    //Correo
    //Clave
    //Reestablecer
    //FechaRegistro
    public class Cliente
    {
        public string Nombres { get; set; }
        public int IdCliente { get; set; }
        public string Apellidos { get; set; }
        public string Clave { get; set; }

        public string Correo { get; set; }

        public bool Reestablecer{ get; set; }

        public string ConfirmarClave { get; set; }

    }
}
