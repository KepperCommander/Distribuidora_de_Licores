using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Nucleo
{
    public class DatosGenerales
    {
        public static string ruta_json =
            @"C:\Users\isara\source\repos\KepperCommander\Distribuidora_de_Licores\Distribuidora_de_Licores\secrets.json";
        public static bool usa_azure = false;
        public static string clave = "EVBgi345936456ghhVBJGtgnifytsidi3456678jhgUTytutyiiyi";
        public static string usuario_datos = EncriptarConversor.Encriptar("Test.Trghhjsgdj");
    }
}
