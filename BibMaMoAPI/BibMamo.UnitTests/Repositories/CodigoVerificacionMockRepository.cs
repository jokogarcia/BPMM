using BibMaMo.Core.Entities;
using BibMaMo.Core.Exceptions;
using BibMaMo.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibMaMo.UnitTests.Repositories
{

  public class CodigoVerificacionMockRepository:ICodigoVerificacionRepository
  {
    static List<CodigoVerificacion> _mockRepo;
    List<CodigoVerificacion> MockRepo { get => _mockRepo ?? (_mockRepo = populateMockRepo()); }

    private List<CodigoVerificacion> populateMockRepo()
    {
      var repo = new List<CodigoVerificacion>();
      for (int x = 0; x < 10; x++)
      {
        repo.Add(new CodigoVerificacion
        {
         Codigo="codigo"+x,
         CodigoId=x,
         ExpirationDate=DateTime.Now.AddDays(x),
         SolicitudId=x
        });
      }
      return repo;
    }


    public Task<CodigoVerificacion> Insert(CodigoVerificacion item)
    {
      item.CodigoId = MockRepo.Count;
      MockRepo.Add(item); 
      return Task.FromResult(item);
    }

    public Task Remove(int id)
    {
      var item = GetItemOrThrow(id);
      MockRepo.Remove(item);
      return Task.CompletedTask;
    }

    public Task<CodigoVerificacion> GetSingle(int id)
    {
      var item =  Task.FromResult(GetItemOrThrow(id));
      checkExpiration(item.Result);
      return item;
    }

  
    private CodigoVerificacion GetItemOrThrow(int id)
    {
      var item = MockRepo.Find(x => x.CodigoId.Equals(id));
      if(item == null)
      {
        throw new ItemNotFoundException();
      }
      return item;
    }

   
    public Task Replace(CodigoVerificacion item)
    {
      var oldCodigoVerificacion = GetItemOrThrow(item.CodigoId);
      var oldCodigoVerificacionIndex = MockRepo.IndexOf(oldCodigoVerificacion);
      MockRepo[oldCodigoVerificacionIndex] = item;
      return Task.CompletedTask;
    }

    public Task<CodigoVerificacion> GetSingleBySolicitudId(int id)
    {
      try
      {
        
        var result = MockRepo.Where(x => x.SolicitudId == id)
          .OrderByDescending(x => x.ExpirationDate)
          .FirstOrDefault();
        if (result == null)
        {
          throw new ItemNotFoundException();
        }
        checkExpiration(result);
        return Task.FromResult(result);
      }
      catch (NullReferenceException)
      {
        throw new ItemNotFoundException();
      }
      catch(Exception e)
      {
        throw;
      }
    }
    private void checkExpiration(CodigoVerificacion item)
    {
      if (item.ExpirationDate > DateTime.Now)
      {
        throw new CodeExpiredException();
      }
    }

    public Task Update(int solicitudId, string newCode)
    {
      MockRepo.OrderByDescending(x => x.ExpirationDate)
        .Where(x => x.SolicitudId == solicitudId)
        .First().Codigo = newCode;
      return Task.CompletedTask;
    }
  }
}
