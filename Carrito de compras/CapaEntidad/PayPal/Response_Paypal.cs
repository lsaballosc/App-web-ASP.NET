using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.PayPal
{
    public class Response_Paypal<T> // Clase genérica para manejar respuestas de PayPal
    {
        public bool Status { get; set; }
        public T Response { get; set; }
    }
}
