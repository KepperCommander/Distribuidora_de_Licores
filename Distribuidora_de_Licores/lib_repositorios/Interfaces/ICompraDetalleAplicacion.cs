using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface ICompraDetalleAplicacion
    {
        void Configurar(string StringConexion);
        List<CompraDetalle> PorCompra(CompraDetalle? entidad); // filtra por CompraId
        List<CompraDetalle> Listar();
        CompraDetalle? Guardar(CompraDetalle? entidad);
        CompraDetalle? Modificar(CompraDetalle? entidad);
        CompraDetalle? Borrar(CompraDetalle? entidad);
    }
}
