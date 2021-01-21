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
    [Fact]

    public async Task GetSingle_WithValidHandle_ReturnsOkAndHasContent()
    {
      var id = await this.GetValidId();
      //Act
      var response = await client.GetAsync($"/api/user/{id}");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<User>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<User>(contentObj);
      Assert.Equal(id, contentObj.UserId);
    }
    [Theory]
    [InlineData(999)]
    public async Task GetSingle_WithInvalidHandle_ReturnsNotFound(int id)
    {
      //Act
      var response = await client.GetAsync($"/api/user/{id}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

 

    [Theory]

    [InlineData("")]
    [InlineData(null)]
    public async Task GetFiltered_WithInvalidTags_ReturnsBadRequest(string tags)
    {
      //Act
      var response = await client.GetAsync($"/api/user/tags/{tags}");
      //assert
      Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }
    [Fact]
    public async Task GetFiltered_WithNonExistingTags_ReturnsNotFound()
    {
      //Act
      var response = await client.GetAsync($"/api/user/tags/non_existing_tag");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    User GenerateValidItem(int id = 0)
    {
      var rnd = new Random().Next(100, 10000);
      User testItem = new User
      {
        UserId = id,
        Email = $"item{rnd}@fakemail.com",
        FirstName = $"User{rnd}First",
        LastName = $"User{rnd}Last]",
        MemberId = rnd < 5 ? string.Empty : rnd.ToString()
      };
      return testItem;
    }
    [Fact]
    public async Task A0InsertItem_ReturnsOk_ReturnsInsertedObj()
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
      // Assert.False(string.IsNullOrEmpty(contentObj.UserId));
    }
    [Fact]
    public async Task A0InsertItem_ItemIsInserted()
    {
      //Prepare
      var requestContent = JsonConvert.SerializeObject(GenerateValidItem());
      var response = await client.PostAsync("/api/user/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<User>(content);
      //act
      response = await client.GetAsync($"/api/user/{contentObj.UserId}");
      var getContent = await response.Content.ReadAsStringAsync();
      var getContentObj = JsonConvert.DeserializeObject<User>(getContent);

      //assert
      Assert.True(response.IsSuccessStatusCode);

      Assert.Equal(contentObj.UserId, getContentObj.UserId);
    }
    [Fact]
    public async Task DeleteItem_ItemIsDeleted()
    {
      //Prepare
      var id = await this.GetValidId();
      //act
      var response = await client.DeleteAsync($"/api/user/{id}");

      //assert
      Assert.True(response.IsSuccessStatusCode);
      response = await client.GetAsync($"/api/user/{id}");
      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    [Fact]
    public async Task DeleteInvalidHAndleItem_ReturnsNotFound()
    {
      //Prepare
      var id = -25;
      //act
      var response = await client.DeleteAsync($"/api/user/{id}");

      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task ModifyItemWithValidHandle_ItemIsModified()
    {
      //Prepare
      var item = GenerateValidItem(await this.GetValidId());
      var requestContent = JsonConvert.SerializeObject(item);
      var response = await client.PutAsync("/api/user/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));

      //act
      var response2 = await client.GetAsync($"/api/user/{item.UserId}");
      var getContent = await response2.Content.ReadAsStringAsync();
      var getContentObj = JsonConvert.DeserializeObject<User>(getContent);

      //assert
      Assert.True(response.IsSuccessStatusCode);

      Assert.Equal(item.UserId, getContentObj.UserId);
      Assert.Equal(item.Email, getContentObj.Email);

    }

    [Fact]
    public async Task ModifyItemWithInvalidHandle_BadRequestReceived()
    {
      //Prepare
      var item = GenerateValidItem();
      item.UserId = -999;
      var requestContent = JsonConvert.SerializeObject(item);
      //act
      var response = await client.PutAsync("/api/user/", new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json"));


      //assert
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
    async Task<int> GetValidId()
    {
      var response = await client.GetAsync("/api/user");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<List<User>>(content);
      if (contentObj.Count > 0)
      {
        return contentObj[new Random().Next(0, contentObj.Count)].UserId;
      }
      throw new Exception();
    }
  }

}
