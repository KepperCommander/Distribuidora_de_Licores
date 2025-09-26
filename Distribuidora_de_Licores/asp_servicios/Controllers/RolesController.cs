using System.Text;
using lib_repositorios.Interfaces;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;      // JsonConversor
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace asp_servicios.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesAplicacion _app;
        private readonly IConfiguration _cfg;
        private readonly TokenController _token; // si usas tu validador propio

        public RolesController(IRolesAplicacion app, IConfiguration cfg, TokenController token)
        {
            _app = app;
            _cfg = cfg;
            _token = token;
        }

        // === Helpers ===
        private Dictionary<string, object> LeerBodyComoDictionary()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var raw = reader.ReadToEnd();
            if (string.IsNullOrWhiteSpace(raw)) raw = "{}";
            return JsonConversor.ConvertirAObjeto<Dictionary<string, object>>(raw)!;
        }

        private string Cnn() =>
            // prioriza "StringConexion" (como tus pruebas), si no usa ConnectionStrings:Default
            _cfg["StringConexion"] ?? _cfg.GetConnectionString("Default") ?? "";

        // === Endpoints ===

        [HttpPost]
        public string Listar()
        {
            var rsp = new Dictionary<string, object>();
            try
            {
                var datos = LeerBodyComoDictionary();
                if (!_token.Validate(datos))
                {
                    rsp["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(rsp);
                }

                _app.Configurar(Cnn());
                rsp["Entidades"] = _app.Listar();
                rsp["Respuesta"] = "OK";
                rsp["Fecha"] = DateTime.Now.ToString("s");
                return JsonConversor.ConvertirAString(rsp);
            }
            catch (Exception ex)
            {
                rsp["Error"] = ex.Message;
                return JsonConversor.ConvertirAString(rsp);
            }
        }

        [HttpPost]
        public string PorNombre()
        {
            var rsp = new Dictionary<string, object>();
            try
            {
                var datos = LeerBodyComoDictionary();
                if (!_token.Validate(datos))
                {
                    rsp["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(rsp);
                }

                // Entrada esperada: { "Entidad": { "Nombre": "Admin" }, "Token": "..." }
                var entidadJson = JsonConversor.ConvertirAString(datos["Entidad"]);
                var entidad = JsonConversor.ConvertirAObjeto<Roles>(entidadJson);

                _app.Configurar(Cnn());
                rsp["Entidades"] = _app.PorNombre(entidad);
                rsp["Respuesta"] = "OK";
                rsp["Fecha"] = DateTime.Now.ToString("s");
                return JsonConversor.ConvertirAString(rsp);
            }
            catch (Exception ex)
            {
                rsp["Error"] = ex.Message;
                return JsonConversor.ConvertirAString(rsp);
            }
        }

        [HttpPost]
        public string Guardar()
        {
            var rsp = new Dictionary<string, object>();
            try
            {
                var datos = LeerBodyComoDictionary();
                if (!_token.Validate(datos))
                {
                    rsp["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(rsp);
                }

                var entidadJson = JsonConversor.ConvertirAString(datos["Entidad"]);
                var entidad = JsonConversor.ConvertirAObjeto<Roles>(entidadJson);

                _app.Configurar(Cnn());
                entidad = _app.Guardar(entidad);
                rsp["Entidad"] = entidad!;
                rsp["Respuesta"] = "OK";
                rsp["Fecha"] = DateTime.Now.ToString("s");
                return JsonConversor.ConvertirAString(rsp);
            }
            catch (Exception ex)
            {
                rsp["Error"] = ex.Message;
                return JsonConversor.ConvertirAString(rsp);
            }
        }

        [HttpPost]
        public string Modificar()
        {
            var rsp = new Dictionary<string, object>();
            try
            {
                var datos = LeerBodyComoDictionary();
                if (!_token.Validate(datos))
                {
                    rsp["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(rsp);
                }

                var entidadJson = JsonConversor.ConvertirAString(datos["Entidad"]);
                var entidad = JsonConversor.ConvertirAObjeto<Roles>(entidadJson);

                _app.Configurar(Cnn());
                entidad = _app.Modificar(entidad);
                rsp["Entidad"] = entidad!;
                rsp["Respuesta"] = "OK";
                rsp["Fecha"] = DateTime.Now.ToString("s");
                return JsonConversor.ConvertirAString(rsp);
            }
            catch (Exception ex)
            {
                rsp["Error"] = ex.Message;
                return JsonConversor.ConvertirAString(rsp);
            }
        }

        [HttpPost]
        public string Borrar()
        {
            var rsp = new Dictionary<string, object>();
            try
            {
                var datos = LeerBodyComoDictionary();
                if (!_token.Validate(datos))
                {
                    rsp["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(rsp);
                }

                var entidadJson = JsonConversor.ConvertirAString(datos["Entidad"]);
                var entidad = JsonConversor.ConvertirAObjeto<Roles>(entidadJson);

                _app.Configurar(Cnn());
                entidad = _app.Borrar(entidad);
                rsp["Entidad"] = entidad!;
                rsp["Respuesta"] = "OK";
                rsp["Fecha"] = DateTime.Now.ToString("s");
                return JsonConversor.ConvertirAString(rsp);
            }
            catch (Exception ex)
            {
                rsp["Error"] = ex.Message;
                return JsonConversor.ConvertirAString(rsp);
            }
        }
    }
}
