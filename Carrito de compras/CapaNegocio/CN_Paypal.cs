using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;   
using Newtonsoft.Json;
using CapaEntidad.PayPal;


namespace CapaNegocio
{
    public class CN_Paypal
    {
        // leer la url y credenciales desde el archivo de configuración
        // Todo esto lo he subido a Github para que se vea el proceso, pero en la vida real no se debe subir a un repositorio público, las subo porque son de prueba

        private static string apiUrl = ConfigurationManager.AppSettings["UrlPaypal"];
        private static string clientId = ConfigurationManager.AppSettings["ClientID"];
        private static string clientSecret = ConfigurationManager.AppSettings["Secret"];



        // metodo para crear solicitud de pago
        public async Task<Response_Paypal<Response_Checkout>> CrearSolicitud(Checkout_Order orden)
        {
            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                var authToken = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));


                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("v2/checkout/orders", data);

                response_paypal.Status = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;

                    Response_Checkout checkout = JsonConvert.DeserializeObject<Response_Checkout>(jsonResponse);
                    response_paypal.Response = checkout;
                }

                return response_paypal;


            }



        }


        // ahora metodo de aprobar pago
        public async Task<Response_Paypal<Response_Capture>> AprobarPago(string token)
        {
            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                var authToken = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));


               
                var data = new StringContent("{}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"v2/checkout/orders/{token}/capture", data);

                response_paypal.Status = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse =  response.Content.ReadAsStringAsync().Result;

                    Response_Capture capture = JsonConvert.DeserializeObject<Response_Capture>(jsonResponse);
                    response_paypal.Response = capture;
                }

                return response_paypal;


            }



        }


    }
}
