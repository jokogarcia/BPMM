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
  public class UserControllerTests
  {
    private UserController _controller;

    User GenerateValidUser(int id = 0)
    {
      var rnd = new Random().Next(100, 10000);
      User testItem = new User()
      {
        FirstName = $"FakeTestName{rnd}",
        LastName = $"FakeTestLastname{rnd}",
        Email = $"FakeTestEmail{rnd}@example.com",
        UserId = id,
        MemberId = rnd % 3 == 0 ? $"MEMBER{rnd}": ""
      };
      return testItem;
    }
    int GetValidHandleFromRepo()
    {
      var items = (_controller.Get().Result as OkObjectResult).Value as List<User>;
      if (items.Count < 1)
      {
        throw new Exception("No items in repo. Test cannot run");
      }
      var randomItem = items[new Random().Next(0, items.Count - 1)];
      return randomItem.UserId;
    }
    public UserControllerTests()
    {
      _controller = new UserController(new UserMockRepository());
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
      var items = Assert.IsType<List<User>>(okResult.Value);
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
      Assert.IsType<User>(okResult.Value);
      Assert.Equal(testHandle, (okResult.Value as User).UserId);
    }
    #endregion
    #region AddMethodTests
    //[Fact]
    //public void Add_InvalidObjectPassed_ReturnsBadRequest()
    //{
    //  // Arrange
    //  var nameMissingItem = new ShoppingItem()
    //  {
    //    Manufacturer = "Guinness",
    //    Price = 12.00M
    //  };
    //  _controller.ModelState.AddModelError("Name", "Required");
    //  // Act
    //  var badResponse = _controller.Post(nameMissingItem);
    //  // Assert
    //  Assert.IsType<BadRequestObjectResult>(badResponse);
    //}
    [Fact]
    public void Add_ValidObjectPassed_ReturnsOkObjectResponse()
    {
      // Arrange
      User testItem = GenerateValidUser();
      // Act
      var createdResponse = _controller.Insert(testItem).Result;
      // Assert
      Assert.IsType<OkObjectResult>(createdResponse);
    }
    [Fact]
    public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
    {
      // Arrange
      User testItem = GenerateValidUser();
      var verifyableProperty = testItem.Email;
      // Act
      var createdResponse = _controller.Insert(testItem).Result as OkObjectResult;
      var item = createdResponse.Value as User;
      // Assert
      Assert.IsType<User>(item);
      Assert.Equal(verifyableProperty, item.Email);
    }
    #endregion
    #region UpdateMethodTests
  
    [Fact]
    public void Update_InvalidHandleObject_ReturnsNotFoundResult()
    {
      // Arrange
      var idMissingItem = GenerateValidUser(-1);
      // Act
      var badResponse = _controller.Replace(idMissingItem).Result;
      // Assert
      Assert.IsType<NotFoundResult>(badResponse);
    }
    [Fact]
    public void Update_ValidObjectPassed_ReturnsOkObjectResponse()
    {
      // Arrange
      var testItem = GenerateValidUser(GetValidHandleFromRepo());
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


  }
}
