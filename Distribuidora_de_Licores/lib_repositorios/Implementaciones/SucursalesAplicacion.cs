using System;
using System.Collections.Generic;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class SucursalesAplicacion : ISucursalesAplicacion
    {
        private IConexion? IConexion;

        public SucursalesAplicacion(IConexion iConexion)
        {
            IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            IConexion!.StringConexion = StringConexion;
        }

        public Sucursales? Guardar(Sucursales? entidad)
        {
            ValidarEntidadParaGuardar(entidad!);

            Normalizar(entidad!);

            // Duplicado por (Nombre, Ciudad) — case-insensitive
            if (IConexion!.Sucursales!.Any(s =>
                  s.Nombre.ToLower() == entidad.Nombre.ToLower() &&
                  s.Ciudad.ToLower() == entidad.Ciudad.ToLower()))
                throw new Exception("Sucursal duplicada (Nombre + Ciudad).");

            IConexion.Sucursales!.Add(entidad);
            IConexion.SaveChanges();
            return entidad;
        }

        public Sucursales? Modificar(Sucursales? entidad)
        {
            ValidarEntidadParaModificar(entidad!);

            Normalizar(entidad!);

            if (IConexion!.Sucursales!.Any(s =>
                    s.SucursalId != entidad.SucursalId &&
                    s.Nombre.ToLower() == entidad.Nombre.ToLower() &&
                    s.Ciudad.ToLower() == entidad.Ciudad.ToLower()))
                throw new Exception("Sucursal duplicada (Nombre + Ciudad).");

            var entry = IConexion!.Entry<Sucursales>(entidad);
            entry.State = EntityState.Modified;
            IConexion.SaveChanges();
            return entidad;
        }

        public Sucursales? Borrar(Sucursales? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.SucursalId == 0) throw new Exception("lbNoSeGuardo");

            IConexion!.Sucursales!.Remove(entidad);
            IConexion.SaveChanges();
            return entidad;
        }

        public List<Sucursales> Listar()
        {
            return IConexion!.Sucursales!
                .OrderBy(s => s.SucursalId)
                .Take(20)
                .ToList();
        }

        public List<Sucursales> PorNombre(Sucursales? entidad)
        {
            var filtro = entidad?.Nombre?.Trim();
            if (string.IsNullOrWhiteSpace(filtro))
                return Listar();

            return IConexion!.Sucursales!
                .Where(s => s.Nombre.Contains(filtro))
                .OrderBy(s => s.SucursalId)
                .Take(20)
                .ToList();
        }

        // ---------- Validaciones / Reglas ----------

        private static void ValidarEntidadParaGuardar(Sucursales entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.SucursalId != 0) throw new Exception("lbYaSeGuardo");
            ValidarCampos(entidad);
        }

        private static void ValidarEntidadParaModificar(Sucursales entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.SucursalId == 0) throw new Exception("lbNoSeGuardo");
            ValidarCampos(entidad);
        }

        private static void ValidarCampos(Sucursales entidad)
        {
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("El Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(entidad.Ciudad))
                throw new Exception("La Ciudad es obligatoria.");
            if (string.IsNullOrWhiteSpace(entidad.Direccion))
                throw new Exception("La Dirección es obligatoria.");

            if (entidad.Nombre.Trim().Length > 80)
                throw new Exception("El Nombre excede 80 caracteres.");
            if (entidad.Ciudad.Trim().Length > 80)
                throw new Exception("La Ciudad excede 80 caracteres.");
            if (entidad.Direccion.Trim().Length > 120)
                throw new Exception("La Dirección excede 120 caracteres.");
        }

        private static void Normalizar(Sucursales entidad)
        {
            entidad.Nombre = entidad.Nombre.Trim();
            entidad.Ciudad = entidad.Ciudad.Trim();
            entidad.Direccion = entidad.Direccion.Trim();
        }
    }
}
