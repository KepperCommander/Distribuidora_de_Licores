using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface ILotesAplicacion
    {
        void Configurar(string StringConexion);
        List<Lotes> PorCodigo(Lotes? entidad); 
        List<Lotes> Listar();
        Lotes? Guardar(Lotes? entidad);
        Lotes? Modificar(Lotes? entidad);
        Lotes? Borrar(Lotes? entidad);
    }
}
