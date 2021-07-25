using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BibMaMo.Infrastructure.Repositories
{
  public class SolicitudInscripcionSocioSqliteRepository : ISolicitudInscripcionSocioRepository
  {



    public async Task<IEnumerable<SolicitudInscripcionSocio>> Get()
    {
      using (var context = new BPMMContext())
      {
        var allItems = context.SolicitudInscripcionSocios.AsAsyncEnumerable();
        return await allItems.ToListAsync();
      }
    }

    public async Task<IEnumerable<SolicitudInscripcionSocio>> GetFiltered(string estado)
    {
     
      using (var context = new BPMMContext())
      {
        try
        {
          var allItems = context.SolicitudInscripcionSocios.AsAsyncEnumerable();
          var result = await allItems.Where(x => x.Estado == estado)
            .ToListAsync();
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
    }
    
    public async Task<IEnumerable<SolicitudInscripcionSocio>> GetInterval(DateTime fromDate, DateTime toDate)
    {
      
      using (var context = new BPMMContext())
      {
        try
        {
          var allItems = context.SolicitudInscripcionSocios.AsAsyncEnumerable();
          var result = await allItems.Where(x => isDateBetween(fromDate,toDate,x.FechaCreacion))
            .ToListAsync();
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
    }

    public Task<SolicitudInscripcionSocio> GetSingle(int id)
    {
      return GetItemOrThrow(id);
    }
    public async Task Approve(int id)
    {
      var item = await GetSingle(id);

      if(item.Estado != "V")
      {
        throw new Exception("Estado invalido para aprobar. Debe ser 'V': verificado. Estado actual: " + NombreDeEstado(item.Estado));
      }
      item.Estado = "A";
      item.FechaProcesamiento = DateTime.Now;
      await Replace(item);
    }

   

    public async Task Reject(int id)
    {
      var item = await GetSingle(id);
      if (item.Estado != "N" && item.Estado != "V")
      {
        throw new Exception("Estado invalido para aprobar. Debe ser 'V': verificado o 'N': nuevo. Estado actual: " + NombreDeEstado(item.Estado));
      }
      item.Estado = "R";
      item.FechaProcesamiento = DateTime.Now;
      await Replace(item);
    }

    public Task<SolicitudInscripcionSocio> Insert(SolicitudInscripcionSocio SolicitudInscripcionSocio)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.SolicitudInscripcionSocios.Add(SolicitudInscripcionSocio);
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Added;
        context.SaveChanges();
        return Task.FromResult(SolicitudInscripcionSocio);

      }
    }

    public async Task Remove(int id)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.SolicitudInscripcionSocios.Remove(await this.GetItemOrThrow(id));
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        await context.SaveChangesAsync();
      }
    }

    public async Task Replace(SolicitudInscripcionSocio SolicitudInscripcionSocio)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.SolicitudInscripcionSocios.Update(SolicitudInscripcionSocio);
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        try
        {
          await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          throw new ItemNotFoundException();
        }
      }
    }
    
    private async Task<SolicitudInscripcionSocio> GetItemOrThrow(int id)
    {
      using (var context = new BPMMContext())
      {
        var item = await context.SolicitudInscripcionSocios.FindAsync(id);
        if (item == null)
        {
          throw new ItemNotFoundException();
        }
        return item;
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
  }
}

