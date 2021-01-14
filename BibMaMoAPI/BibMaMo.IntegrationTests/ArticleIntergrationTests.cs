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
    [Fact]
    public async Task GetSingle_WithValidHandle_ReturnsOkAndHasContent()
    {
      //Act
      var response = await client.GetAsync("/api/article/1");
      var content = await response.Content.ReadAsStringAsync();
      var contentObj = JsonConvert.DeserializeObject<Article>(content);
      //assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.IsType<Article>(contentObj);
      Assert.Equal("1",contentObj.Handle);
    }
  }

}
