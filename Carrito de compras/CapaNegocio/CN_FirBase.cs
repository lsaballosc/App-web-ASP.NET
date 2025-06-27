using Firebase.Auth;
using Firebase.Storage;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_FirBase
    {
        public async Task<string> SubirStorage(Stream archivo, string nombreArchivo)
        {
            string url_image = string.Empty;

            // datos para usar el servicio de Firebase
            string email = "EMAIL";
            string password = "PASS";
            string ruta = "ROUTEp"; // ⚠️ este es el BUCKET, no el dominio
            string apikey = "APIKEY";

            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(apikey));

                var a = await auth.SignInWithEmailAndPasswordAsync(email, password);
                Console.WriteLine("TOKEN OBTENIDO: " + a.FirebaseToken);

                var cancellation = new CancellationTokenSource();


                var task = new FirebaseStorage(ruta, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("IMAGEN_PRODUCTOS")
                .Child(nombreArchivo)
                .PutAsync(archivo, cancellation.Token);

                url_image = await task;




                return url_image;
            }
            catch (Exception ex)
            {
                throw new Exception( "Erro al subir la imagen" + ex );
            }
        }
    }
}

