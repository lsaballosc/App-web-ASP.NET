﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuario
    {

        //        IdUsuario
        //Nombres
        //Apellidos
        //Correo
        //Clave
        //Reestablecer
        //Activo
        //FechaRegistro

        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public bool Reestablecer { get; set; }
        public bool Activo { get; set; }

        public String FechaRegistro { get; set; }
    }
}
