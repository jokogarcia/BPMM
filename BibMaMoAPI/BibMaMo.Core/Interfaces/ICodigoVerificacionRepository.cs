using BibMaMo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface ICodigoVerificacionRepository
  {

    Task<CodigoVerificacion> GetSingle(int id);
    Task<CodigoVerificacion> GetSingleBySolicitudId(int id);
    Task Remove(int id);
    Task<CodigoVerificacion> Insert(CodigoVerificacion CodigoVerificacion);
    Task Replace(CodigoVerificacion CodigoVerificacion);
    Task Update(int solicitudId, string newCode);
  }
}
