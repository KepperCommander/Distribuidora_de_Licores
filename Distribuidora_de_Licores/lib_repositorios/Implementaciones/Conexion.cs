using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace lib_repositorios.Implementaciones
{
    public partial class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }
        public string? UsuarioActual { get; set; }
        public Conexion(DbContextOptions<Conexion> options) : base(options) { }

        public Conexion()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(this.StringConexion!, p => { });
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.StringConexion!, p => { });
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }

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
        public DbSet<Auditoria>? Auditoria { get; set; }


        //logica para auditoria 

        public override int SaveChanges()
        {
            var entradas = ChangeTracker.Entries()
                .Where(e => !(e.Entity is Auditoria) &&
                           (e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted))
                .ToList();

            // guarda
            var logs = new List<Auditoria>();
            foreach (var e in entradas)
            {
                var tabla = e.Metadata.GetTableName() ?? e.Entity.GetType().Name;

                var pk = string.Join(",",
                    e.Properties
                     .Where(p => p.Metadata.IsKey())
                     .Select(p => $"{p.Metadata.Name}={(p.CurrentValue ?? p.OriginalValue ?? "-")}")
                );

                logs.Add(new Auditoria
                {
                    Tabla = tabla,
                    Llave = string.IsNullOrWhiteSpace(pk) ? "-" : pk,
                    Accion = e.State.ToString()
                });
            }

            //inserta las auditorías ya fuera del foreach original
            if (logs.Count > 0)
                Set<Auditoria>().AddRange(logs);

            return base.SaveChanges();
        }



    }
}
