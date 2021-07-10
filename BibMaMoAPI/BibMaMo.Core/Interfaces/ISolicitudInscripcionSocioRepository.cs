using BibMaMo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BibMaMo.Core.Interfaces
{
  public interface ISolicitudInscripcionSocioRepository
  {
    Task<IEnumerable<SolicitudInscripcionSocio>> Get();
    Task<IEnumerable<SolicitudInscripcionSocio>> GetFiltered(string estado);
    Task<SolicitudInscripcionSocio> GetSingle(int id);
    Task Remove(int id);
    Task<SolicitudInscripcionSocio> Insert(SolicitudInscripcionSocio SolicitudInscripcionSocio);
    Task Replace(SolicitudInscripcionSocio SolicitudInscripcionSocio);
  }
}