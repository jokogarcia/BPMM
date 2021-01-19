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
  public class UserIntergrationTests
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
      var response = await client.GetAsync("/api/user");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<List<User>>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<List<User>>(contentObj);


    }
    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    public async Task GetSingle_WithValidHandle_ReturnsOkAndHasContent(string handle)
    {
      //Act
      var response = await client.GetAsync($"/api/user/{handle}");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<User>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<User>(contentObj);
      Assert.Equal(handle, contentObj.Handle);
    }
    [Theory]
    [InlineData("fhwh098")]
    public async Task GetSingle_WithInvalidHandle_ReturnsNotFound(string handle)
    {
      //Act
      var response = await client.GetAsync($"/api/user/{handle}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    

    

    [Theory]
    [InlineData("fhwh098")]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetFiltered_WithInvalidTags_ReturnsNotFound(string tags)
    {
      //Act
      var response = await client.GetAsync($"/api/user/tags/{tags}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    User GenerateValidItem(string handle = "")
    {
      var rnd = new Random().Next(100, 10000);
      User testItem = new User
      {
        Handle = rnd.ToString(),
        Email = $"item{rnd}@fakemail.com",
        FirstName = $"User{rnd}First",
        LastName = $"User{rnd}Last]",
        MemberId = rnd < 5 ? string.Empty : rnd.ToString()
      };
      return testItem;
    }
    [Fact]
    public async Task InsertItem_ReturnsOk_ReturnsInsertedObj()
    {
      //Prepare
      var requestContent = JsonConvert.SerializeObject(GenerateValidItem());

      //act
      var response = await client.PostAsync("/api/user/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));

      //assert
      Assert.True(response.IsSuccessStatusCode);
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<User>(content);
      Assert.NotNull(contentObj);
      Assert.False(string.IsNullOrEmpty(contentObj.Handle));
    }
    [Fact]
    public async Task InsertItem_ItemIsInserted()
    {
      //Prepare
      var requestContent = JsonConvert.SerializeObject(GenerateValidItem());
      var response = await client.PostAsync("/api/user/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<User>(content);
      //act
      response = await client.GetAsync($"/api/user/{contentObj.Handle}");
      var getContent = await response.Content.ReadAsStringAsync();
      var getContentObj = JsonConvert.DeserializeObject<User>(getContent);

      //assert
      Assert.True(response.IsSuccessStatusCode);

      Assert.Equal(contentObj.Handle, getContentObj.Handle);
    }
    [Fact]
    public async Task DeleteItem_ItemIsDeleted()
    {
      //Prepare
      var handle = "7";
      //act
      var response = await client.DeleteAsync($"/api/user/{handle}");

      //assert
      Assert.True(response.IsSuccessStatusCode);
      response = await client.GetAsync($"/api/user/{handle}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    [Fact]
    public async Task DeleteInvalidHAndleItem_ReturnsNotFound()
    {
      //Prepare
      var handle = "1fgejbkasdf0";
      //act
      var response = await client.DeleteAsync($"/api/user/{handle}");

      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task ModifyItemWithValidHandle_ItemIsModified()
    {
      //Prepare
      var item = GenerateValidItem();
      item.Handle = "4";
      var requestContent = JsonConvert.SerializeObject(item);
      var response = await client.PutAsync("/api/user/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));

      //act
      var response2 = await client.GetAsync($"/api/user/{item.Handle}");
      var getContent = await response2.Content.ReadAsStringAsync();
      var getContentObj = JsonConvert.DeserializeObject<User>(getContent);

      //assert
      Assert.True(response.IsSuccessStatusCode);

      Assert.Equal(item.Handle, getContentObj.Handle);
      Assert.Equal(item.Email, getContentObj.Email);

    }

    [Fact]
    public async Task ModifyItemWithInValidHandle_BadRequestReceived()
    {
      //Prepare
      var item = GenerateValidItem();
      item.Handle = "invalid";
      var requestContent = JsonConvert.SerializeObject(item);
      //act
      var response = await client.PutAsync("/api/user/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));


      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

  }

}
