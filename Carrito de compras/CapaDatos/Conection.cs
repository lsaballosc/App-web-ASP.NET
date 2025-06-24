using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace CapaDatos
{
    public class Conection
    {
        // Conexión obtenida desde el archivo de configuración
        public static string cn = ConfigurationManager.ConnectionStrings["Conexion"].ToString();


    }
}
