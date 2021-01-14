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
  public class ArticleControllerTests
  {
    private ArticleController _controller;
    public ArticleControllerTests()
    {
      _controller = new ArticleController(new ArticleMockRepository());
    }
    #region GetAllMethodTests
    [Fact]
    public void Get_WhenCalled_ReturnsOkResult()
    {
      // Act
      var okResult = _controller.GetArticles();
      // Assert
      Assert.IsType<OkObjectResult>(okResult.Result);
    }

    [Fact]
    public void Get_WhenCalled_ReturnsAllItems()
    {
      // Act
      var okResult = _controller.GetArticles().Result as OkObjectResult;
      // Assert
      var items = Assert.IsType<List<Article>>(okResult.Value);
      Assert.True(items.Count > 0);
    }
    #endregion
    #region GetSingleMethodTests
    [Fact]
    public void GetSingle_UnknownHandlePassed_ReturnsNotFoundResult()
    {
      // Act
      var notFoundResult = _controller.GetSingleArticle("bad handle");
      // Assert
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    [Fact]
    public void GetSingle_EmptyHandlePassed_ReturnsNotFoundResult()
    {
      // Act
      var notFoundResult = _controller.GetSingleArticle("");
      // Assert
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    [Fact]
    public void GetSingle_NullHandlePassed_ReturnsNotFoundResult()
    {
      // Act
      var notFoundResult = _controller.GetSingleArticle(null);
      // Assert
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    [Fact]
    public void GetSingle_ExistingHandlePassed_ReturnsOkResult()
    {
      // Arrange
      // Act
      var okResult = _controller.GetSingleArticle("1");
      // Assert
      Assert.IsType<OkObjectResult>(okResult.Result);
    }
    [Fact]
    public void GetSingle_ExistingHandlePassed_ReturnsRightItem()
    {
      // Arrange
      var testHandle = "1";
      // Act
      var okResult = _controller.GetSingleArticle(testHandle).Result as OkObjectResult;
      // Assert
      Assert.IsType<Article>(okResult.Value);
      Assert.Equal(testHandle, (okResult.Value as Article).Handle);
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
      Article testItem = new Article()
      {
        Title = "Guinness Original 6 Pack",
        HtmlContent = "Guinness",
        Handle = "",
        MainImageUrl="test.jpg",
        Tags="some,tags"
      };
      // Act
      var createdResponse = _controller.InsertArticle(testItem).Result;
      // Assert
      Assert.IsType<OkObjectResult>(createdResponse);
    }
    [Fact]
    public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
    {
      // Arrange
      Article testItem = new Article()
      {
        Title = "Guinness Original 6 Pack",
        HtmlContent = "Guinness",
        Handle = "",
        MainImageUrl = "test.jpg",
        Tags = "some,tags"
      };
      // Act
      var createdResponse = _controller.InsertArticle(testItem).Result as OkObjectResult;
      var item = createdResponse.Value as Article;
      // Assert
      Assert.IsType<Article>(item);
      Assert.Equal("Guinness Original 6 Pack", item.Title);
      Assert.False(string.IsNullOrEmpty(item.Handle));
    }
    #endregion
    #region UpdateMethodTests
    [Fact]
    public void Update_EmptyHandleObject_ReturnsNotFoundResult()
    {
      // Arrange
      var handleMissingItem = new Article()
      {
        Title = "Guinness Original 6 Pack",
        HtmlContent = "Guinness",
        Handle = "",
        MainImageUrl = "test.jpg",
        Tags = "some,tags"
      };
      // Act
      var badResponse = _controller.UpdateArticle(handleMissingItem).Result;
      // Assert
      Assert.IsType<NotFoundResult>(badResponse);
    }
    [Fact]
    public void Update_InvalidHandleObject_ReturnsNotFoundResult()
    {
      // Arrange
      var handleMissingItem = new Article()
      {
        Title = "Guinness Original 6 Pack",
        HtmlContent = "Guinness",
        Handle = "An Invalid Handle",
        MainImageUrl = "test.jpg",
        Tags = "some,tags"
      };
      // Act
      var badResponse = _controller.UpdateArticle(handleMissingItem).Result;
      // Assert
      Assert.IsType<NotFoundResult>(badResponse);
    }
    [Fact]
    public void Update_ValidObjectPassed_ReturnsOkObjectResponse()
    {
      // Arrange
      Article testItem = new Article()
      {
        Title = "Updated in a test",
        HtmlContent = "Guinness. Updated.",
        Handle = "3",
        MainImageUrl = "test.jpg",
        Tags = "some,tags"
      };
      // Act
      var createdResponse = _controller.UpdateArticle(testItem).Result;
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
      var badResponse = _controller.DeleteArticle(notExistinHgandle).Result;
      // Assert
      Assert.IsType<NotFoundResult>(badResponse);
    }
    [Fact]
    public void Remove_ExistingHandlePassed_ReturnsOkResult()
    {
      // Arrange
      var existingHandle = "5";
      // Act
      var okResponse = _controller.DeleteArticle(existingHandle).Result;
      // Assert
      Assert.IsType<OkResult>(okResponse);
    }
    [Fact]
    public void Remove_ExistingHandlePassed_RemovesOneItem()
    {
      // Arrange
      var existingHandle = "6";
      
      // Act
      _ = _controller.DeleteArticle(existingHandle);
      // Assert
      Assert.IsType<NotFoundResult>(_controller.GetSingleArticle(existingHandle).Result) ;
    }
    #endregion


  }
}
