using BibMaMo.Api.Controllers;
using BibMaMo.Core.Entities;
using BibMaMo.Core.Interfaces;
using BibMaMo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BibMamo.UnitTests
{
  public class BookControllerTests
  {
    private BookController _controller;

    Book GenerateValidBook(string handle = "")
    {
      var rnd = new Random().Next(100, 10000);
      Book testItem = new Book()
      {
        Title = $"Test title {rnd}",
        Descriptor = rnd % 2 == 0 ? "novela" : "teatro",
        Handle = handle,
        Author = $"Test Author {rnd}.jpg",
        Tags = $"test tag,rnd{rnd}",
        Collection = rnd % 2 == 0 ? "" : "La ciudad de los naranjos",
        Edition = 1990 + rnd % 30,
        InventoryId = $"INV{rnd}",
        ISBN = $"73215684{rnd}",
        Pages = rnd % 1000,
        Publisher = $"Test Publisher {rnd}",
        Section = rnd % 2 == 0 ? "arriba" : "abajo",
        Summary = $"Lorem ipsum doloor, test generated summary {rnd}"
      };
      return testItem;
    }
    string GetValidHandleFromRepo()
    {
      var items = (_controller.Get().Result as OkObjectResult).Value as List<Book>;
      var randomItem = items[new Random().Next(0, items.Count - 1)];
      return randomItem.Handle;
    }
    public BookControllerTests()
    {
      _controller = new BookController(new BookMockRepository());
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
      var items = Assert.IsType<List<Book>>(okResult.Value);
      Assert.True(items.Count > 0);
    }
    #endregion
    #region GetSingleMethodTests
    [Fact]
    public void GetSingle_UnknownHandlePassed_ReturnsNotFoundResult()
    {
      // Act
      var notFoundResult = _controller.GetSingle("bad handle");
      // Assert
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    [Fact]
    public void GetSingle_EmptyHandlePassed_ReturnsNotFoundResult()
    {
      // Act
      var notFoundResult = _controller.GetSingle("");
      // Assert
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    [Fact]
    public void GetSingle_NullHandlePassed_ReturnsNotFoundResult()
    {
      // Act
      var notFoundResult = _controller.GetSingle(null);
      // Assert
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    [Fact]
    public void GetSingle_ExistingHandlePassed_ReturnsOkResult()
    {
      // Arrange
      var handle = GetValidHandleFromRepo();
      // Act
      var okResult = _controller.GetSingle(handle);
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
      Assert.IsType<Book>(okResult.Value);
      Assert.Equal(testHandle, (okResult.Value as Book).Handle);
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
      Book testItem = GenerateValidBook();
      // Act
      var createdResponse = _controller.Insert(testItem).Result;
      // Assert
      Assert.IsType<OkObjectResult>(createdResponse);
    }
    [Fact]
    public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
    {
      // Arrange
      Book testItem = GenerateValidBook();
      var verifyableProperty = testItem.Title;
      // Act
      var createdResponse = _controller.Insert(testItem).Result as OkObjectResult;
      var item = createdResponse.Value as Book;
      // Assert
      Assert.IsType<Book>(item);
      Assert.Equal(verifyableProperty, item.Title);
      Assert.False(string.IsNullOrEmpty(item.Handle));
    }
    #endregion
    #region UpdateMethodTests
    [Fact]
    public void Update_EmptyHandleObject_ReturnsNotFoundResult()
    {
      // Arrange
      var handleMissingItem = GenerateValidBook();
      // Act
      var badResponse = _controller.Replace(handleMissingItem).Result;
      // Assert
      Assert.IsType<NotFoundResult>(badResponse);
    }
    [Fact]
    public void Update_InvalidHandleObject_ReturnsNotFoundResult()
    {
      // Arrange
      var handleMissingItem = GenerateValidBook();
      // Act
      var badResponse = _controller.Replace(handleMissingItem).Result;
      // Assert
      Assert.IsType<NotFoundResult>(badResponse);
    }
    [Fact]
    public void Update_ValidObjectPassed_ReturnsOkObjectResponse()
    {
      // Arrange
      var testItem = GenerateValidBook(GetValidHandleFromRepo());
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
      var notExistinHgandle = "not existing Handle";
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
    public void GetFiltered_UnknownTagsPassed_ReturnsNotFoundResult()
    {
      // Act
      var notFoundResult = _controller.GetFiltered("iojfoewm,oifwe,jne");
      // Assert
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    [Fact]
    public void GetFiltered_EmptyTagsPassed_ReturnsBadRequest()
    {
      // Act
      var badRequestResult = _controller.GetFiltered("");
      // Assert
      Assert.IsType<BadRequestResult>(badRequestResult.Result);
    }
    [Fact]
    public void GetFiltered_NullTagsPassed_ReturnsBadRequestResult()
    {
      // Act
      var badRequestResult = _controller.GetFiltered(null);
      // Assert
      Assert.IsType<BadRequestResult>(badRequestResult.Result);
    }
    [Fact]
    public void GetFiltered_ExistingTagPassed_ReturnsOkResult()
    {
      // Arrange
      // Act
      var okResult = _controller.GetFiltered("tag1");
      // Assert
      Assert.IsType<OkObjectResult>(okResult.Result);
    }
    [Fact]
    public void GetFiltered_ExistingTagPassed_ReturnsRightItems()
    {
      // Arrange
      var testTag = "tag2";
      // Act
      var okResult = _controller.GetFiltered(testTag).Result as OkObjectResult;
      var resultValue = okResult.Value as List<Book>;
      // Assert
      Assert.IsType<List<Book>>(okResult.Value);
      Assert.Null(resultValue.Find(x => !x.Tags.Contains(testTag))); //Check that all the items contain the testTag
    }
    [Fact]
    public void GetFiltered_ExistingTagsPassed_ReturnsOkResult()
    {
      // Arrange
      // Act
      var okResult = _controller.GetFiltered("tag1,tag2");
      // Assert
      Assert.IsType<OkObjectResult>(okResult.Result);
    }
    [Fact]
    public void GetFiltered_ExistingTagsPassed_ReturnsRightItems()
    {
      // Arrange
      var testTags = "tag1,tag2";
      var testTagsArr = testTags.Split(',');
      // Act
      var okResult = _controller.GetFiltered(testTags).Result as OkObjectResult;
      var resultValue = okResult.Value as List<Book>;
      // Assert
      Assert.IsType<List<Book>>(okResult.Value);
      Assert.Null(resultValue.Find(x => !x.Tags.Contains(testTagsArr[0]) && !x.Tags.Contains(testTags[1]))); //Check that all the items contain at least one testTag
    }
    #endregion

  }
}
