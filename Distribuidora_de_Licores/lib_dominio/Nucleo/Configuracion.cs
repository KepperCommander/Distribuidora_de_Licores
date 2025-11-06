
namespace lib_dominio.Nucleo
{
    public class Configuracion
    {
        public static Dictionary<string, string>? datos = null;

        public static string ObtenerValor(string clave)
        {
            if (datos == null)
                Cargar();

            if (datos == null || !datos.ContainsKey(clave))
                throw new Exception($"No se encontró la clave '{clave}' en secrets.json.");

            var valor = datos[clave];
            return valor ?? string.Empty;
        }

        public static void Cargar()
        {
            try
            {
                var ruta = DatosGenerales.ruta_json;
                if (!File.Exists(ruta))
                    throw new Exception($"No se encontró el archivo de configuración: {ruta}");

                string json = File.ReadAllText(ruta);
                datos = JsonConversor.ConvertirAObjeto<Dictionary<string, string>>(json);

                if (datos == null)
                    throw new Exception("Error al leer el contenido del secrets.json.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error cargando configuración: {ex.Message}");
            }
        }
    }
}
