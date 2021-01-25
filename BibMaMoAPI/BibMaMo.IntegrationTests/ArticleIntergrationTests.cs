using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System;
using Xunit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using BibMaMo.Core.Entities;
using Newtonsoft.Json;
using System.Net;
using System.Linq;

namespace BibMaMo.IntegrationTests
{
  public class ArticleIntergrationTests
  {
    HttpClient _client;
    HttpClient client { get => _client ?? (_client = GetClientAsync().Result); }
    async Task<HttpClient> GetClientAsync()
    {
      var hostBuilder = new HostBuilder()
        .ConfigureWebHost(webHost =>
        {
          // Add TestServer
          webHost.UseTestServer();
          webHost.UseStartup<BibMaMoAPI.Startup>();
          // Specify the environment

          webHost.UseEnvironment("Test");

         // webHost.Configure(app => app.Run(async ctx => await ctx.Response.WriteAsync("Hello World!")));

        });
      var host = await hostBuilder.StartAsync();
      return host.GetTestClient();

    }
    [Fact]
    public async Task Get_ReturnsOkAndHasContent()
    {
      //Act
      var response = await client.GetAsync("/api/article");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<List<Article>>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<List<Article>>(contentObj);


    }
    [Fact]
   
    public async Task GetSingle_WithValidHandle_ReturnsOkAndHasContent()
    {
      var id = await this.GetValidId();
      //Act
      var response = await client.GetAsync($"/api/article/{id}");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<Article>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<Article>(contentObj);
      Assert.Equal(id,contentObj.ArticleId);
    }
    [Theory]
    [InlineData(999)]
    public async Task GetSingle_WithInvalidHandle_ReturnsNotFound(int id)
    {
      //Act
      var response = await client.GetAsync($"/api/article/{id}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
      
    }

    [Theory]
    [InlineData("some")]
    [InlineData("tags")]
    [InlineData("some,tags")]
    public async Task GetFiltered_WithValidTags_ReturnsOkAndHasContent(string tags)
    {
      //Act
      var response = await client.GetAsync($"/api/article/tags/{tags}");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<List<Article>>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<List<Article>>(contentObj);
      Assert.True(contentObj.TrueForAll(x => ItemContainsTag(x, tags)));
    }

    private bool ItemContainsTag( Article item, string tags)
    {
      var tagsArr = tags.ToLower().Split(',');
      var itemTagsArr = item.Tags.ToLower().Split(',');
      foreach (var tag in tagsArr)
      {
        if (itemTagsArr.Contains(tag))
          return true;
      }
      return false;
    }

    [Theory]
    
    [InlineData("")]
    [InlineData(null)]
    public async Task GetFiltered_WithInvalidTags_ReturnsBadRequest(string tags)
    {
      //Act
      var response = await client.GetAsync($"/api/article/tags/{tags}");
      //assert
      Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }
    [Fact]
    public async Task GetFiltered_WithNonExistingTags_ReturnsNotFound()
    {
      //Act
      var response = await client.GetAsync($"/api/article/tags/non_existing_tag");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    Article GenerateValidItem(int id=0)
    {
      var rnd = new Random().Next(100, 10000);
      Article testItem = new Article()
      {
        Title = $"Test title {rnd}",
        Subtitle ="A subtitle",
        Author="Fake author",
        HtmlContent = $"<p>This is randomly generated <b>HTML</b> content id <i>{rnd}</i>",
        ArticleId = id,
        MainImageUrl = $"test{rnd}.jpg",
        Tags = $"some,tags,test,rnd{rnd}"
      };
      return testItem;
    }
    [Fact]
    public async Task A0InsertItem_ReturnsOk_ReturnsInsertedObj()
    {
      //Prepare
      var requestContent = JsonConvert.SerializeObject(GenerateValidItem());

      //act
      var response = await client.PostAsync("/api/article/", new StringContent(requestContent,System.Text.Encoding.UTF8,"application/json"));

      //assert
      Assert.True(response.IsSuccessStatusCode);
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<Article>(content);
      Assert.NotNull(contentObj);
     // Assert.False(string.IsNullOrEmpty(contentObj.ArticleId));
    }
    [Fact]
    public async Task A0InsertItem_ItemIsInserted()
    {
      //Prepare
      var requestContent = JsonConvert.SerializeObject(GenerateValidItem());
      var response = await client.PostAsync("/api/article/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<Article>(content);
      //act
      response = await client.GetAsync($"/api/article/{contentObj.ArticleId}");
      var getContent = await response.Content.ReadAsStringAsync();
      var getContentObj = JsonConvert.DeserializeObject<Article>(getContent);
      
      //assert
      Assert.True(response.IsSuccessStatusCode);
      
      Assert.Equal(contentObj.ArticleId,getContentObj.ArticleId);
    }
    [Fact]
    public async Task DeleteItem_ItemIsDeleted()
    {
      //Prepare
      var id = await this.GetValidId();
      //act
      var response = await client.DeleteAsync($"/api/article/{id}");
      
      //assert
      Assert.True(response.IsSuccessStatusCode);
      response = await client.GetAsync($"/api/article/{id}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    [Fact]
    public async Task DeleteInvalidHAndleItem_ReturnsNotFound()
    {
      //Prepare
      var id =-25;
      //act
      var response = await client.DeleteAsync($"/api/article/{id}");

      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task ModifyItemWithValidHandle_ItemIsModified()
    {
      //Prepare
      var item = GenerateValidItem(await this.GetValidId());
      var requestContent = JsonConvert.SerializeObject(item);
      var response = await client.PutAsync("/api/article/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));

      //act
      var response2 = await client.GetAsync($"/api/article/{item.ArticleId}");
      var getContent = await response2.Content.ReadAsStringAsync();
      var getContentObj = JsonConvert.DeserializeObject<Article>(getContent);

      //assert
      Assert.True(response.IsSuccessStatusCode);

      Assert.Equal(item.ArticleId, getContentObj.ArticleId);
      Assert.Equal(item.Title, getContentObj.Title);

    }

    [Fact]
    public async Task ModifyItemWithInvalidHandle_BadRequestReceived()
    {
      //Prepare
      var item = GenerateValidItem();
      item.ArticleId = -999;
      var requestContent = JsonConvert.SerializeObject(item);
      //act
      var response = await client.PutAsync("/api/article/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));
      

      //assert
      Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);

    }
    async Task<int> GetValidId()
    {
      var response = await client.GetAsync("/api/article");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<List<Article>>(content);
      if (contentObj.Count > 0)
      {
        return contentObj[new Random().Next(0, contentObj.Count)].ArticleId;
      }
      throw new Exception();
    }
  }

}
