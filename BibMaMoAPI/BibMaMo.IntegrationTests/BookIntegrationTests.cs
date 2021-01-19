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
  public class BookIntergrationTests
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
      var response = await client.GetAsync("/api/book");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<List<Book>>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<List<Book>>(contentObj);


    }
    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    public async Task GetSingle_WithValidHandle_ReturnsOkAndHasContent(string handle)
    {
      //Act
      var response = await client.GetAsync($"/api/book/{handle}");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<Book>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<Book>(contentObj);
      Assert.Equal(handle, contentObj.Handle);
    }
    [Theory]
    [InlineData("fhwh098")]
    public async Task GetSingle_WithInvalidHandle_ReturnsNotFound(string handle)
    {
      //Act
      var response = await client.GetAsync($"/api/book/{handle}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Theory]
    [InlineData("tag1")]
    [InlineData("tag2")]
    [InlineData("tag1,tag2")]
    public async Task GetFiltered_WithValidTags_ReturnsOkAndHasContent(string tags)
    {
      //Act
      var response = await client.GetAsync($"/api/book/tags/{tags}");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<List<Book>>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<List<Book>>(contentObj);
      Assert.True(contentObj.TrueForAll(x => ItemContainsTag(x, tags)));
    }

    private bool ItemContainsTag(Book item, string tags)
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
      var response = await client.GetAsync($"/api/book/tags/{tags}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    Book GenerateValidItem(string handle = "")
    {
      var rnd = new Random().Next(100, 10000);
      Book testItem = new Book
      {
        Handle = rnd.ToString(),
        Author = $"Autor {rnd}",
        Title = $"Libro {rnd}",
        Tags = "tag2",
        Descriptor = "poesia",
        Edition = 2000 + rnd,
        ISBN = Guid.NewGuid().ToString(),
        Pages = 700 + rnd * 17,
        InventoryId = $"INV{(300 + rnd)}",
        Publisher = rnd % 2 == 0 ? "Alfaguara" : "Kapeluz",
        Section = rnd % 2 == 0 ? "Seccion A" : "Seccion B",
        Summary = "Lorem ipsum doloor."
      };
      return testItem;
    }
    [Fact]
    public async Task InsertItem_ReturnsOk_ReturnsInsertedObj()
    {
      //Prepare
      var requestContent = JsonConvert.SerializeObject(GenerateValidItem());

      //act
      var response = await client.PostAsync("/api/book/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));

      //assert
      Assert.True(response.IsSuccessStatusCode);
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<Book>(content);
      Assert.NotNull(contentObj);
      Assert.False(string.IsNullOrEmpty(contentObj.Handle));
    }
    [Fact]
    public async Task InsertItem_ItemIsInserted()
    {
      //Prepare
      var requestContent = JsonConvert.SerializeObject(GenerateValidItem());
      var response = await client.PostAsync("/api/book/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<Book>(content);
      //act
      response = await client.GetAsync($"/api/book/{contentObj.Handle}");
      var getContent = await response.Content.ReadAsStringAsync();
      var getContentObj = JsonConvert.DeserializeObject<Book>(getContent);

      //assert
      Assert.True(response.IsSuccessStatusCode);

      Assert.Equal(contentObj.Handle, getContentObj.Handle);
    }
    [Fact]
    public async Task DeleteItem_ItemIsDeleted()
    {
      //Prepare
      var handle = "10";
      //act
      var response = await client.DeleteAsync($"/api/book/{handle}");

      //assert
      Assert.True(response.IsSuccessStatusCode);
      response = await client.GetAsync($"/api/book/{handle}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    [Fact]
    public async Task DeleteInvalidHAndleItem_ReturnsNotFound()
    {
      //Prepare
      var handle = "1fgejbkasdf0";
      //act
      var response = await client.DeleteAsync($"/api/book/{handle}");

      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task ModifyItemWithValidHandle_ItemIsModified()
    {
      //Prepare
      var item = GenerateValidItem();
      item.Handle = "12";
      var requestContent = JsonConvert.SerializeObject(item);
      var response = await client.PutAsync("/api/book/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));

      //act
      var response2 = await client.GetAsync($"/api/book/{item.Handle}");
      var getContent = await response2.Content.ReadAsStringAsync();
      var getContentObj = JsonConvert.DeserializeObject<Book>(getContent);

      //assert
      Assert.True(response.IsSuccessStatusCode);

      Assert.Equal(item.Handle, getContentObj.Handle);
      Assert.Equal(item.Title, getContentObj.Title);

    }

    [Fact]
    public async Task ModifyItemWithInValidHandle_BadRequestReceived()
    {
      //Prepare
      var item = GenerateValidItem();
      item.Handle = "invalid";
      var requestContent = JsonConvert.SerializeObject(item);
      //act
      var response = await client.PutAsync("/api/book/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));


      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

  }

}
