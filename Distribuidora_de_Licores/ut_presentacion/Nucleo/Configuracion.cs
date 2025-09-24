using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using lib_dominio.Nucleo;

namespace ut_presentacion.Nucleo
{
    public static class Configuracion
    {
        private static Dictionary<string, string>? datos;
        private static string? origenCargado;

        public static string ObtenerValor(string clave)
        {
            if (datos == null) Cargar();
            if (datos == null)
                throw new InvalidOperationException(
                    $"No se pudo cargar configuración. Busqué en: {origenCargado ?? "<sin ruta>"}");

            if (!datos.TryGetValue(clave, out var valor) || string.IsNullOrWhiteSpace(valor))
                throw new KeyNotFoundException($"Clave '{clave}' no encontrada en configuración ({origenCargado}).");

            return valor;
        }

        public static void Cargar()
        {
            var start = AppDomain.CurrentDomain.BaseDirectory;

            // 1) Buscar hacia arriba secrets.json
            var path = FindUpFor("secrets.json", start, maxLevels: 15);
            if (path != null && File.Exists(path))
            {
                origenCargado = path;
                datos = LeerJson(path);
                return;
            }

            // 2) Buscar .sln hacia arriba y probar secrets.json en la raíz de la solución
            var sln = FindUpForExtension("*.sln", start, maxLevels: 15);
            if (sln != null && File.Exists(sln))
            {
                var root = Path.GetDirectoryName(sln)!;
                var candidate = Path.Combine(root, "secrets.json");
                if (File.Exists(candidate))
                {
                    origenCargado = candidate;
                    datos = LeerJson(candidate);
                    return;
                }
            }

            // 3) Fallback: variable de entorno con la cadena completa
            var env = Environment.GetEnvironmentVariable("StringConexion");
            if (!string.IsNullOrWhiteSpace(env))
            {
                datos = new Dictionary<string, string> { ["StringConexion"] = env };
                origenCargado = "ENV:StringConexion";
                return;
            }

            // Si todo falla, deja pista de dónde buscó
            origenCargado = path ?? (sln ?? (start + "secrets.json"));
            datos = null;
        }

        private static Dictionary<string, string> LeerJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConversor.ConvertirAObjeto<Dictionary<string, string>>(json)!;
        }

        private static string? FindUpFor(string fileName, string startDir, int maxLevels = 12)
        {
            var dir = new DirectoryInfo(startDir);
            for (int i = 0; i < maxLevels && dir != null; i++, dir = dir.Parent!)
            {
                var candidate = Path.Combine(dir.FullName, fileName);
                if (File.Exists(candidate)) return candidate;
            }
            return null;
        }

        private static string? FindUpForExtension(string pattern, string startDir, int maxLevels = 12)
        {
            var dir = new DirectoryInfo(startDir);
            for (int i = 0; i < maxLevels && dir != null; i++, dir = dir.Parent!)
            {
                try
                {
                    var match = dir.GetFiles(pattern).FirstOrDefault();
                    if (match != null) return match.FullName;
                }
                catch { /* ignorar accesos denegados */ }
            }
            return null;
        }
    }
}
