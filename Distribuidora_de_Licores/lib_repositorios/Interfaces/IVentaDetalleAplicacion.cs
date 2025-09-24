using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IVentaDetalleAplicacion
    {
        void Configurar(string StringConexion);
        List<VentaDetalle> PorVenta(VentaDetalle? entidad); // filtra por VentaId
        List<VentaDetalle> Listar();
        VentaDetalle? Guardar(VentaDetalle? entidad);
        VentaDetalle? Modificar(VentaDetalle? entidad);
        VentaDetalle? Borrar(VentaDetalle? entidad);
    }

}
