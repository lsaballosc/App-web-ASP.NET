using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {


        private CD_ubicacion objUbi = new CD_ubicacion();
       

        public List<Provincia> ObtenerProvincia()
        {
            return objUbi.ObtenerProvincia();
        }

        public List<Canton> ObtenerCanton(string idprovincia)
        {
            return objUbi.ObtenerCanton(idprovincia);
        }

        public List<Distrito> ObtenerDistrito(string idprovincia, string idcan)
        {
            return objUbi.ObtenerDistrito(idprovincia, idcan);
        }


        public Distrito ObtenerDatosUbicacionPorDistrito(string idDistrito)
        {
            return  objUbi.ObtenerDatosUbicacionPorDistrito(idDistrito);
        }

    }
}
