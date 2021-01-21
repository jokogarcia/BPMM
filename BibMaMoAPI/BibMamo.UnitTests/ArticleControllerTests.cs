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
  public class ArticleControllerTests
  {
    private ArticleController _controller;

    Article GenerateValidArticle(int id=0)
    {
      var rnd = new Random().Next(100,10000);
      Article testItem = new Article()
      {
        Title = $"Test title {rnd}",
        HtmlContent = $"<p>This is randomly generated <b>HTML</b> content id <i>{rnd}</i>",
        ArticleId = id,
        MainImageUrl = $"test{rnd}.jpg",
        Tags = $"some,tags,test,rnd{rnd}"
      };
      return testItem;
    }
    int GetValidHandleFromRepo()
    {
      var items = (_controller.Get().Result as OkObjectResult).Value as List<Article>;
      var itemsCount = items.Count;
      if (itemsCount < 1)
      {
        throw new Exception("No items in repository");
      }
      var randomItem = items[new Random().Next(0, itemsCount - 1)];
      return randomItem.ArticleId;
    }
    public ArticleControllerTests()
    {
      _controller = new ArticleController(new ArticleMockRepository());
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
      var items = Assert.IsType<List<Article>>(okResult.Value);
      Assert.True(items.Count > 0);
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
      var okResult = _controller.GetFiltered("tags");
      // Assert
      Assert.IsType<OkObjectResult>(okResult.Result);
    }
    [Fact]
    public void GetFiltered_ExistingTagPassed_ReturnsRightItems()
    {
      // Arrange
      var testTag = "some";
      // Act
      var okResult = _controller.GetFiltered(testTag).Result as OkObjectResult;
      var resultValue = okResult.Value as List<Article>;
      // Assert
      Assert.IsType<List<Article>>(okResult.Value);
      Assert.Null(resultValue.Find(x=>!x.Tags.Contains(testTag))); //Check that all the items contain the testTag
    }
    [Fact]
    public void GetFiltered_ExistingTagsPassed_ReturnsOkResult()
    {
      // Arrange
      // Act
      var okResult = _controller.GetFiltered("some,tags");
      // Assert
      Assert.IsType<OkObjectResult>(okResult.Result);
    }
    [Fact]
    public void GetFiltered_ExistingTagsPassed_ReturnsRightItems()
    {
      // Arrange
      var testTags = "some,tags";
      var testTagsArr = testTags.Split(',');
      // Act
      var okResult = _controller.GetFiltered(testTags).Result as OkObjectResult;
      var resultValue = okResult.Value as List<Article>;
      // Assert
      Assert.IsType<List<Article>>(okResult.Value);
      var itemsWithNoneOfTheTags = resultValue.Find(x => !x.Tags.Contains(testTagsArr[0]) && !x.Tags.Contains(testTagsArr[1]));
      Assert.Null(itemsWithNoneOfTheTags); //Check that all the items contain at least one testTag
    }
    #endregion
    
    
    #region GetSingleMethodTests
    [Fact]
    public void GetSingle_UnknownHandlePassed_ReturnsNotFoundResult()
    {
      // Act
      var notFoundResult = _controller.GetSingle(-25);
      // Assert
      Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }
    //[Fact]
    //public void GetSingle_EmptyHandlePassed_ReturnsNotFoundResult()
    //{
    //  // Act
    //  var notFoundResult = _controller.GetSingle();
    //  // Assert
    //  Assert.IsType<NotFoundResult>(notFoundResult.Result);
    //}
    //[Fact]
    //public void GetSingle_NullHandlePassed_ReturnsNotFoundResult()
    //{
    //  // Act
    //  var notFoundResult = _controller.GetSingle(null);
    //  // Assert
    //  Assert.IsType<NotFoundResult>(notFoundResult.Result);
    //}
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
      Assert.IsType<Article>(okResult.Value);
      Assert.Equal(testHandle, (okResult.Value as Article).ArticleId);
    }
    #endregion
    #region AddMethodTests
    
    [Fact]
    public void Add_ValidObjectPassed_ReturnsOkObjectResponse()
    {
      // Arrange
      Article testItem = GenerateValidArticle();
      // Act
      var createdResponse = _controller.Insert(testItem).Result;
      // Assert
      Assert.IsType<OkObjectResult>(createdResponse);
    }
    [Fact]
    public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
    {
      // Arrange
      Article testItem = GenerateValidArticle();
      var verifyableProperty = testItem.Title;
      // Act
      var createdResponse = _controller.Insert(testItem).Result as OkObjectResult;
      var item = createdResponse.Value as Article;
      // Assert
      Assert.IsType<Article>(item);
      Assert.Equal(verifyableProperty, item.Title);
     // Assert.False(string.IsNullOrEmpty(item.ArticleId));
    }
    #endregion
    #region UpdateMethodTests
    //[Fact]
    //public void Update_EmptyHandleObject_ReturnsNotFoundResult()
    //{
    //  // Arrange
    //  var idMissingItem = GenerateValidArticle();
    //  // Act
    //  var badResponse = _controller.Replace(idMissingItem).Result;
    //  // Assert
    //  Assert.IsType<NotFoundResult>(badResponse);
    //}
    [Fact]
    public void Update_InvalidHandleObject_ReturnsNotFoundResult()
    {
      // Arrange
      var idMissingItem = GenerateValidArticle(-20);
      // Act
      var badResponse = _controller.Replace(idMissingItem).Result;
      // Assert
      Assert.IsType<NotFoundResult>(badResponse);
    }
    [Fact]
    public void Update_ValidObjectPassed_ReturnsOkObjectResponse()
    {
      // Arrange
      var testItem = GenerateValidArticle(GetValidHandleFromRepo());
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
      var notExistinHgandle = -25;
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
      Assert.IsType<NotFoundResult>(_controller.GetSingle(existingHandle).Result) ;
    }
    #endregion
    

  }
}
