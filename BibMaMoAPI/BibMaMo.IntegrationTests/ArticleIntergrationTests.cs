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
    HttpClient client { get => _client ?? GetClientAsync().Result; }
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
    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    public async Task GetSingle_WithValidHandle_ReturnsOkAndHasContent(string handle)
    {
      //Act
      var response = await client.GetAsync($"/api/article/{handle}");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<Article>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<Article>(contentObj);
      Assert.Equal(handle,contentObj.Handle);
    }
    [Theory]
    [InlineData("fhwh098")]
    public async Task GetSingle_WithInvalidHandle_ReturnsNotFound(string handle)
    {
      //Act
      var response = await client.GetAsync($"/api/article/{handle}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
      
    }

    [Theory]
    [InlineData("nuestros")]
    [InlineData("coleccion")]
    [InlineData("nuestros,coleccion")]
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
    [InlineData("fhwh098")]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetFiltered_WithInvalidTags_ReturnsNotFound(string tags)
    {
      //Act
      var response = await client.GetAsync($"/api/article/tags/{tags}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    Article GenerateValidArticle(string handle = "")
    {
      var rnd = new Random().Next(100, 10000);
      Article testItem = new Article()
      {
        Title = $"Test title {rnd}",
        HtmlContent = $"<p>This is randomly generated <b>HTML</b> content id <i>{rnd}</i>",
        Handle = handle,
        MainImageUrl = $"test{rnd}.jpg",
        Tags = $"some,tags,test,rnd{rnd}"
      };
      return testItem;
    }
    [Fact]
    public async Task InsertItem_ReturnsOk_ReturnsInsertedObj()
    {
      //Prepare
      var requestContent = JsonConvert.SerializeObject(GenerateValidArticle());

      //act
      var response = await client.PostAsync("/api/article/", new StringContent(requestContent,System.Text.Encoding.UTF8,"application/json"));

      //assert
      Assert.True(response.IsSuccessStatusCode);
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<Article>(content);
      Assert.NotNull(contentObj);
      Assert.False(string.IsNullOrEmpty(contentObj.Handle));
    }
    [Fact]
    public async Task InsertItem_ItemIsInserted()
    {
      //Prepare
      var requestContent = JsonConvert.SerializeObject(GenerateValidArticle());
      var response = await client.PostAsync("/api/article/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<Article>(content);
      //act
      response = await client.GetAsync($"/api/article/{contentObj.Handle}");
      var getContent = await response.Content.ReadAsStringAsync();
      var getContentObj = JsonConvert.DeserializeObject<Article>(content);
      
      //assert
      Assert.True(response.IsSuccessStatusCode);
      
      Assert.Equal(contentObj,getContentObj);
    }
  }

}
