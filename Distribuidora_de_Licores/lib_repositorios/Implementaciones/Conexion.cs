using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace lib_repositorios.Implementaciones
{
    public partial class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.StringConexion!, p => { });
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
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
    }
}
