using BibMaMo.Api.Controllers;
using BibMaMo.Core.Entities;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Repositories;
using BibMaMo.UnitTests.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BibMamo.UnitTests
{
  public class SolicitudInscripcionSocioControllerTests
  {
    private SolicitudInscripcionSocioController _controller;

    SolicitudInscripcionSocio GenerateValidSolicitudInscripcionSocio(int id = 0)
    {
      SolicitudInscripcionSocio testItem = Helpers.GeneratorHelpers.generateRandomSolicitud(id);
      return testItem;
    }
    int GetValidHandleFromRepo()
    {
      var items = (_controller.Get().Result as OkObjectResult).Value as List<SolicitudInscripcionSocio>;
      if (items.Count < 1)
      {
        throw new Exception("No items in repo. Test cannot run");
      }
      var randomItem = items[new Random().Next(0, items.Count - 1)];
      return randomItem.SolicitudId;
    }
    public SolicitudInscripcionSocioControllerTests()
    {
      _controller = new SolicitudInscripcionSocioController(new SolicitudInscripcionSocioMockRepository(), new CodigoVerificacionMockRepository(),new MailerService.MailerServiceMock());
    }
    #region GetAllMethodTests
    [Fact]
    public void Get_WhenCalled_ReturnsOkResult()
    {
      // Act
      var okResult = _controller.Get();
      // Assert
      Assert.IsType<OkObjectResult>(okResult.Result);
    }

    [Fact]
    public void Get_WhenCalled_ReturnsAllItems()
    {
      // Act
      var okResult = _controller.Get().Result as OkObjectResult;
      // Assert
      var items = Assert.IsType<List<SolicitudInscripcionSocio>>(okResult.Value);
      Assert.True(items.Count > 0);
    }
    #endregion
    #region GetSingleMethodTests
    [Fact]
    public void GetSingle_UnknownHandlePassed_ReturnsNotFoundResult()
    {
      // Act
      var notFoundResult = _controller.GetSingle(-1);
      // Assert
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }


    [Fact]
    public void GetSingle_ExistingHandlePassed_ReturnsOkResult()
    {
      // Arrange
      var id = GetValidHandleFromRepo();
      // Act
      var okResult = _controller.GetSingle(id);
      // Assert
      Assert.IsType<OkObjectResult>(okResult.Result);
    }
    [Fact]
    public void GetSingle_ExistingHandlePassed_ReturnsRightItem()
    {
      // Arrange
      var testHandle = GetValidHandleFromRepo();
      // Act
      var okResult = _controller.GetSingle(testHandle).Result as OkObjectResult;
      // Assert
      Assert.IsType<SolicitudInscripcionSocio>(okResult.Value);
      Assert.Equal(testHandle, (okResult.Value as SolicitudInscripcionSocio).SolicitudId);
    }
    #endregion
    #region AddMethodTests
    [Fact]
    public void Add_ValidObjectPassed_ReturnsOkObjectResponse()
    {
      // Arrange
      SolicitudInscripcionSocio testItem = GenerateValidSolicitudInscripcionSocio();
      // Act
      var createdResponse = _controller.Insert(testItem).Result;
      // Assert
      Assert.IsType<OkObjectResult>(createdResponse);
    }
    [Fact]
    public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
    {
      // Arrange
      SolicitudInscripcionSocio testItem = GenerateValidSolicitudInscripcionSocio();
      var verifyableProperty = testItem.DniNum;
      // Act
      var createdResponse = _controller.Insert(testItem).Result as OkObjectResult;
      var item = createdResponse.Value as SolicitudInscripcionSocio;
      // Assert
      Assert.IsType<SolicitudInscripcionSocio>(item);
      Assert.Equal(verifyableProperty, item.DniNum);

    }
    #endregion
    #region UpdateMethodTests

    [Fact]
    public void Update_InvalidHandleObject_ReturnsNotFoundResult()
    {
      // Arrange
      var idMissingItem = GenerateValidSolicitudInscripcionSocio(981);
      // Act
      var badResponse = _controller.Replace(idMissingItem).Result;
      // Assert
      Assert.IsType<NotFoundResult>(badResponse);
    }
    [Fact]
    public void Update_ValidObjectPassed_ReturnsOkObjectResponse()
    {
      // Arrange
      var testItem = GenerateValidSolicitudInscripcionSocio(GetValidHandleFromRepo());
      // Act
      var createdResponse = _controller.Replace(testItem).Result;
      // Assert
      Assert.IsType<OkResult>(createdResponse);
    }

    #endregion
    #region DeleteActionTests
    [Fact]
    public void Remove_NotExistingHandlePassed_ReturnsNotFoundResponse()
    {
      // Arrange
      var notExistinHgandle = -1;
      // Act
      var badResponse = _controller.Remove(notExistinHgandle).Result;
      // Assert
      Assert.IsType<NotFoundResult>(badResponse);
    }
    [Fact]
    public void Remove_ExistingHandlePassed_ReturnsOkResult()
    {
      // Arrange
      var existingHandle = GetValidHandleFromRepo();
      // Act
      var okResponse = _controller.Remove(existingHandle).Result;
      // Assert
      Assert.IsType<OkResult>(okResponse);
    }
    [Fact]
    public void Remove_ExistingHandlePassed_RemovesOneItem()
    {
      // Arrange
      var existingHandle = GetValidHandleFromRepo();

      // Act
      _ = _controller.Remove(existingHandle);
      // Assert
      Assert.IsType<NotFoundResult>(_controller.GetSingle(existingHandle).Result);
    }
    #endregion

    #region GetFilteredMethodTests
    [Fact]
    public void GetFiltered_UnknownEstadoPassed_ReturnsNotFoundResult()
    {
      // Act
      var notFoundResult = _controller.GetFiltered("i");
      // Assert
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    [Fact]
    public void GetFiltered_EmptyEstadoPassed_ReturnsBadRequest()
    {
      // Act
      var badRequestResult = _controller.GetFiltered("");
      // Assert
      Assert.IsType<BadRequestResult>(badRequestResult.Result);
    }
    [Fact]
    public void GetFiltered_NullEstadoPassed_ReturnsBadRequestResult()
    {
      // Act
      var badRequestResult = _controller.GetFiltered(null).Result;
      // Assert
      Assert.IsType<BadRequestResult>(badRequestResult);
    }
    [Fact]
    public void GetFiltered_ExistingEstadoPassed_ReturnsOkResult()
    {
      // Arrange
      // Act
      var okResult =  _controller.GetFiltered("N").Result;
      // Assert
      Assert.IsType<OkObjectResult>(okResult);
    }
    [Fact]
    public void GetFiltered_ExistingEstadoPassed_ReturnsRightItems()
    {
      // Arrange
      var testEstado = "N";
      var testEstadoArr = testEstado.Split(',');
      // Act
      var okResult = _controller.GetFiltered(testEstado).Result as OkObjectResult;
      var resultValue = okResult.Value as List<SolicitudInscripcionSocio>;
      // Assert
      Assert.IsType<List<SolicitudInscripcionSocio>>(okResult.Value);
      var itemsWithNoneOfTheEstado = resultValue.Find(x => !x.Estado.Contains(testEstadoArr[0]) && !x.Estado.Contains(testEstadoArr[1]));
      Assert.Null(itemsWithNoneOfTheEstado); //Check that all the items contain at least one testTag
    }
    #endregion
    #region AceptarTests
    [Fact]
    public void ZAcceptInvalidId_ReturnsNotFoundResult()
    {
      var notFoundResult = _controller.Approve(-1);
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    #endregion
    #region Intervalo
    [Fact]
    public void GetIntervalo_ReturnsNotFoundResult()
    {
      var notFoundResult = _controller.GetInterval(DateTime.MinValue, DateTime.MinValue.AddDays(2));
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    [Fact]
    public void GetIntervalo_ReturnsAtLeastOne()
    {
      var okResult = _controller.GetInterval(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(2)).Result as OkObjectResult;
      var resultValue = okResult.Value as List<SolicitudInscripcionSocio>;
      Assert.IsType<List<SolicitudInscripcionSocio>>(okResult.Value);
    }

    [Fact]
    public void GetIntervalo_ReturnsCorrectValues()
    {
      var minDate = DateTime.Now.AddDays(-1);
      var maxDate = DateTime.Now.AddDays(1);
      var okResult = _controller.GetInterval(minDate,maxDate).Result as OkObjectResult;
      var resultValue = okResult.Value as List<SolicitudInscripcionSocio>;
      foreach(var item in resultValue)
      {
        Assert.True(item.FechaCreacion >= minDate && item.FechaCreacion <= maxDate);
      }
    }
    #endregion

    #region ResendCode
    [Fact]
    public void ResendCode_NotCrash()
    {
      var okResult = _controller.ResendCode(1);
      Assert.IsType<OkResult>(okResult.Result);
    }
    [Fact]
    public void ResendCode_ObjectNotFound()
    {
      var notFoundResult = _controller.ResendCode(199999);
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    #endregion

    #region VerifyCode
    [Fact]
    public void VerifyCode_OkResult()
    {

      var okResult = _controller.VerifyCode(0, "codigo0").Result;
      Assert.IsType<OkResult>(okResult);
    }
    [Fact]
    public void VerifyCode_BadRequestResult()
    {

      var okResult = _controller.VerifyCode(2, "codigobad1");
      Assert.IsType<BadRequestObjectResult>(okResult.Result);
    }
    [Fact]
    public void VerifyCode_ObjectNotFound()
    {
      var notFoundResult = _controller.VerifyCode(199999,"cue");
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    #endregion
  }
}
