using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace lib_repositorios.Interfaces
{
    public interface IConexion
    {
        string? StringConexion { get; set; }

        public DbSet<Roles>? Roles { get; set; }
        public DbSet<Usuarios>? Usuarios { get; set; }
        public DbSet<Sucursales>? Sucursales { get; set; }
        public DbSet<Empleados>? Empleados { get; set; }
        public DbSet<Proveedores>? Proveedores { get; set; }
        public DbSet<Categorias>? Categorias { get; set; }
        public DbSet<Marcas>? Marcas { get; set; }
        public DbSet<Productos>? Productos { get; set; }
        public DbSet<Clientes>? Clientes { get; set; }
        public DbSet<Inventario>? Inventario { get; set; }
        public DbSet<Lotes>? Lotes { get; set; }
        public DbSet<Compras>? Compras { get; set; }
        public DbSet<CompraDetalle>? CompraDetalle { get; set; }
        public DbSet<Ventas>? Ventas { get; set; }
        public DbSet<VentaDetalle>? VentaDetalle { get; set; }

        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}