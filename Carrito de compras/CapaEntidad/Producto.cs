using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
//    IdProducto
//Nombre
//Descripción
//IdMarca
//IdCategoria
//Precio
//Stock
//RutaImagen
//NombreImagen
//Activo
//Fecha Registro
    public class Producto
    {

        public int IdProducto { get; set; }
        public  String Nombre { get; set; }
        public Marca oMarca { get; set; }

        public Categoria oCategoria { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }
        public string RutaImagen { get; set; }

        public string NombreImagen { get; set; }

        public bool Activo { get; set; }
        public string Descripcion { get; set; }

        // para guardar con decimal
        public string PrecioTexto { get; set; }
        //para guardar la base 64 de la imagen y su extension
        public string Base64 { get; set; }
        public string Extension { get; set; }
    }
}
