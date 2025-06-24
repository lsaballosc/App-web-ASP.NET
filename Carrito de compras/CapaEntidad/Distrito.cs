using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Distrito
    {

//        IdDistrito
//  Descripción



        public string IdDistrito { get; set; }

        public string Descripcion { get; set; }
        public string ProvinciaDescripcion { get; set; }
        public string CantonDescripcion { get; set; }
    }
}
