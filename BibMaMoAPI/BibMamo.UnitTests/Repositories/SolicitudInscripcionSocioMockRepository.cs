using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibMamo.UnitTests.Helpers;

namespace BibMaMo.UnitTests.Repositories
{

  public class SolicitudInscripcionSocioMockRepository : ISolicitudInscripcionSocioRepository
  {
    static List<SolicitudInscripcionSocio> _mockRepo;
    List<SolicitudInscripcionSocio> MockRepo { get => _mockRepo ?? (_mockRepo = populateMockRepo()); }

    
    
    private static int datosIds=0;
    
    
    private List<SolicitudInscripcionSocio> populateMockRepo()
    {
      var MockRepo = new List<SolicitudInscripcionSocio>();
      MockRepo.Add(new SolicitudInscripcionSocio
      {
        Apellido="Garcia",
        Nombre="Joaquin",
        DniNum="30415559",
        DniTipo="DNI",
        Email="jokogarcia@gmail.com",
        Estado="N",
        FechaCreacion=DateTime.Now,
        Fnac=new DateTime(1984,07,29),
        Nota="Lorem ipsum",
        SolicitudId=0,
        TipoSolicitud="CADETE",
        DatosContactoLaboral= BibMamo.UnitTests.Helpers.GeneratorHelpers.randomDatosContactos(1)

      });;
      for (int x = 1; x < 11; x++)
      {
        MockRepo.Add(GeneratorHelpers.generateRandomSolicitud(x));
      }
      return MockRepo;
    }


    public Task<SolicitudInscripcionSocio> Insert(SolicitudInscripcionSocio item)
    {
      item.SolicitudId = new Random().Next(1, 100);
      MockRepo.Add(item);

      return Task.FromResult(item);
    }

    public Task Remove(int id)
    {
      var item = GetItemOrThrow(id);
      MockRepo.Remove(item);
      return Task.CompletedTask;
    }

    public Task<SolicitudInscripcionSocio> GetSingle(int id)
    {
      return Task.FromResult(GetItemOrThrow(id));
    }

    public Task<IEnumerable<SolicitudInscripcionSocio>> Get()
    {
      IEnumerable<SolicitudInscripcionSocio> items;
      items = (IEnumerable<SolicitudInscripcionSocio>)MockRepo;
      return Task.FromResult(items);

    }
    private SolicitudInscripcionSocio GetItemOrThrow(int id)
    {
      var item = MockRepo.Find(x => x.SolicitudId.Equals(id));
      if (item == null)
      {
        throw new ItemNotFoundException();
      }
      return item;
    }
    public Task Replace(SolicitudInscripcionSocio item)
    {
      var oldSolicitudInscripcionSocio = GetItemOrThrow(item.SolicitudId);
      var oldSolicitudInscripcionSocioIndex = MockRepo.IndexOf(oldSolicitudInscripcionSocio);
      MockRepo[oldSolicitudInscripcionSocioIndex] = item;
      return Task.CompletedTask;
    }

    public async Task<IEnumerable<SolicitudInscripcionSocio>> GetFiltered(string estado)
    {
      try
      {
        var allItems = MockRepo;
        var result = allItems.Where(x => x.Estado == estado)
          .ToList();
        if (!result.Any())
        {
          throw new ItemNotFoundException();
        }
        return result;
      }
      catch (NullReferenceException)
      {
        throw new ItemNotFoundException();
      }
    }
    
    public async Task<IEnumerable<SolicitudInscripcionSocio>> GetInterval(DateTime fromDate, DateTime toDate)
    {
      try
      {
        var allItems = MockRepo;
        var result = allItems.Where(x => isDateBetween(fromDate, toDate, x.FechaCreacion))
          .ToList();
        if (!result.Any())
        {
          throw new ItemNotFoundException();
        }
        return result;
      }
      catch (NullReferenceException)
      {
        throw new ItemNotFoundException();
      }
    }

    public async Task Reject(int id)
    {
      var item = await GetSingle(id);
      if (item.Estado != "N")
      {
        throw new Exception("Esta solicitud ya fue procesada. Estado actual: " + NombreDeEstado(item.Estado));
      }
      item.Estado = "A";
      item.FechaProcesamiento = DateTime.Now;
      await Replace(item);
    }

    public async Task Approve(int id)
    {
      var item = await GetSingle(id);
      if (item.Estado != "N")
      {
        throw new Exception("Esta solicitud ya fue procesada. Estado actual: " + NombreDeEstado(item.Estado));
      }
      item.Estado = "A";
      item.FechaProcesamiento = DateTime.Now;
      await Replace(item);
    }

    private static string NombreDeEstado(string estado)
    {
      switch (estado.ToUpper())
      {
        case "N": return "NUEVO";
        case "R": return "RECHAZADO";
        case "A": return "APROBADO";
        default: return $"DESCONOCIDO / INVALIDO ({estado})";
      }
    }
    private static bool isDateBetween(DateTime fromDate, DateTime toDate, DateTime chequingDate)
    {
      if (fromDate > toDate)
      {
        throw new Exception("Fecha inicial no puede ser mayor que la final");
      }
      return chequingDate.Ticks > fromDate.Ticks && chequingDate.Ticks <= toDate.Ticks;
    }
  }
}
