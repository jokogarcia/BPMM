using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BibMaMo.Infrastructure.Repositories
{
  public class CodigoVerificacionSqliteRepository : ICodigoVerificacionRepository
  {

    public async Task<CodigoVerificacion> GetSingle(int id)
    {
      var item = await GetItemOrThrow(id);
      checkExpiration(item);
      return item;
    }
    public async Task<CodigoVerificacion> GetSingleBySolicitudId(int solicitudId)
    {
      var item = await getSingleBySolicitudId(solicitudId);
      checkExpiration(item);
      return item;
    }

    private void checkExpiration(CodigoVerificacion item)
    {
      if (item.ExpirationDate > DateTime.Now)
      {
        throw new CodeExpiredException();
      }
    }

    public Task<CodigoVerificacion> Insert(CodigoVerificacion codigoVerificacion)
    {
      codigoVerificacion.ExpirationDate = DateTime.Now.AddDays(1);
      using (var context = new BPMMContext())
      {
        var entity = context.CodigoVerificacions.Add(codigoVerificacion);
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Added;
        context.SaveChanges();
        return Task.FromResult(codigoVerificacion);
      }
    }

    public async Task Remove(int id)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.CodigoVerificacions.Remove(await this.GetItemOrThrow(id));
        entity.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        await context.SaveChangesAsync();
      }
    }

    public async Task Replace(CodigoVerificacion CodigoVerificacion)
    {
      using (var context = new BPMMContext())
      {
        var entity = context.CodigoVerificacions.Update(CodigoVerificacion);
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
    public async Task Update(int solicitudId, string newCode)
    {
      var item = await getSingleBySolicitudId(solicitudId);
      item.Codigo = newCode;
      item.ExpirationDate = DateTime.Now.AddDays(1);
      await this.Replace(item);
    }
    private async Task<CodigoVerificacion> getSingleBySolicitudId(int solicitudId)
    {
      using (var context = new BPMMContext())
      {
        try
        {
          var allItems = context.CodigoVerificacions.AsAsyncEnumerable();
          var result = await allItems.Where(x => x.SolicitudId == solicitudId)
            .OrderByDescending(x => x.ExpirationDate)
            .FirstAsync();
          if (result == null)
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
    private async Task<CodigoVerificacion> GetItemOrThrow(int id)
    {
      using (var context = new BPMMContext())
      {
        var item = await context.CodigoVerificacions.FindAsync(id);
        if (item == null)
        {
          throw new ItemNotFoundException();
        }
        return item;
      }

    }
  }
}

