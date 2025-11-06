using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using lib_repositorios.Implementaciones;
using Microsoft.AspNetCore.Mvc;

namespace asp_servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductosController : ControllerBase
    {
        private IProductosAplicacion? iAplicacion = null;
        private TokenAplicacion? iAplicacionToken = null;

        public ProductosController(IProductosAplicacion? iAplicacion, TokenAplicacion iAplicacionToken)
        {
            this.iAplicacion = iAplicacion;
            this.iAplicacionToken = iAplicacionToken;
        }

        private Dictionary<string, object> ObtenerDatos()
        {
            var datos = new StreamReader(Request.Body).ReadToEnd();
            if (string.IsNullOrWhiteSpace(datos)) datos = "{}";
            return JsonConversor.ConvertirAObjeto(datos);
        }

        [HttpPost]
        public string Listar()
        {
            var r = new Dictionary<string, object>();
            try
            {
                var d = ObtenerDatos();
                if (!iAplicacionToken!.Validar(d))
                { r["Error"] = "lbNoAutenticacion"; return JsonConversor.ConvertirAString(r); }

                iAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));

                r["Entidades"] = iAplicacion!.Listar();
                r["Respuesta"] = "OK";
                r["Fecha"] = DateTime.Now.ToString();
                return JsonConversor.ConvertirAString(r);
            }
            catch (Exception ex) { r["Error"] = ex.Message; r["Respuesta"] = "Error"; return JsonConversor.ConvertirAString(r); }
        }

        [HttpPost]
        public string PorNombre()
        {
            var r = new Dictionary<string, object>();
            try
            {
                var d = ObtenerDatos();
                if (!iAplicacionToken!.Validar(d))
                { r["Error"] = "lbNoAutenticacion"; return JsonConversor.ConvertirAString(r); }

                var entidad = JsonConversor.ConvertirAObjeto<Productos>(JsonConversor.ConvertirAString(d["Entidad"]));
                iAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));

                r["Entidades"] = iAplicacion!.PorNombre(entidad);
                r["Respuesta"] = "OK";
                r["Fecha"] = DateTime.Now.ToString();
                return JsonConversor.ConvertirAString(r);
            }
            catch (Exception ex) { r["Error"] = ex.Message; r["Respuesta"] = "Error"; return JsonConversor.ConvertirAString(r); }
        }

        [HttpPost]
        public string Guardar()
        {
            var r = new Dictionary<string, object>();
            try
            {
                var d = ObtenerDatos();
                if (!iAplicacionToken!.Validar(d))
                { r["Error"] = "lbNoAutenticacion"; return JsonConversor.ConvertirAString(r); }

                var entidad = JsonConversor.ConvertirAObjeto<Productos>(JsonConversor.ConvertirAString(d["Entidad"]));
                iAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));

                entidad = iAplicacion!.Guardar(entidad);
                r["Entidad"] = entidad!;
                r["Respuesta"] = "OK";
                r["Fecha"] = DateTime.Now.ToString();
                return JsonConversor.ConvertirAString(r);
            }
            catch (Exception ex) { r["Error"] = ex.Message; r["Respuesta"] = "Error"; return JsonConversor.ConvertirAString(r); }
        }

        [HttpPost]
        public string Modificar()
        {
            var r = new Dictionary<string, object>();
            try
            {
                var d = ObtenerDatos();
                if (!iAplicacionToken!.Validar(d))
                { r["Error"] = "lbNoAutenticacion"; return JsonConversor.ConvertirAString(r); }

                var entidad = JsonConversor.ConvertirAObjeto<Productos>(JsonConversor.ConvertirAString(d["Entidad"]));
                iAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));

                entidad = iAplicacion!.Modificar(entidad);
                r["Entidad"] = entidad!;
                r["Respuesta"] = "OK";
                r["Fecha"] = DateTime.Now.ToString();
                return JsonConversor.ConvertirAString(r);
            }
            catch (Exception ex) { r["Error"] = ex.Message; r["Respuesta"] = "Error"; return JsonConversor.ConvertirAString(r); }
        }

        [HttpPost]
        public string Borrar()
        {
            var r = new Dictionary<string, object>();
            try
            {
                var d = ObtenerDatos();
                if (!iAplicacionToken!.Validar(d))
                { r["Error"] = "lbNoAutenticacion"; return JsonConversor.ConvertirAString(r); }

                var entidad = JsonConversor.ConvertirAObjeto<Productos>(JsonConversor.ConvertirAString(d["Entidad"]));
                iAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));

                entidad = iAplicacion!.Borrar(entidad);
                r["Entidad"] = entidad!;
                r["Respuesta"] = "OK";
                r["Fecha"] = DateTime.Now.ToString();
                return JsonConversor.ConvertirAString(r);
            }
            catch (Exception ex) { r["Error"] = ex.Message; r["Respuesta"] = "Error"; return JsonConversor.ConvertirAString(r); }
        }
    }
}
