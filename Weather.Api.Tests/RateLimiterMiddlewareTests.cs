using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Weather.Api.RateLimit;
using Weather.Contracts;
using Xunit;

namespace Weather.Api.Tests;

public class RateLimiterMiddlewareTests
{
    [Fact]
    public async Task WithoutApiKey_ReturnsUnauthorized()
    {
        // Arrange
        using var host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
           {
               webBuilder
               .UseTestServer()
               .ConfigureServices(services =>
               {
                   //services.AddControllers();
                   //services.AddSingleton<IWeatherService, WeatherService>();

                   //services.Add(new WeatherService(new WeatherServiceConfig { ApiKeys = new string[] { }, ApiUrl = "" }));
                   //services.AddSingleton<IWeatherService, WeatherService>();
               })
               .Configure(app =>
               {
                   app.UseRateLimiterMiddleware();
                   //app.UseRateLimiterMiddleware();
               });
           }).StartAsync();
        // Act
        var response = await host.GetTestClient().GetAsync("/");
        // Assert
        //Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var server = host.GetTestServer();
        server.BaseAddress = new Uri("https://localhost");
        var context = await server.SendAsync(c =>
        {
            c.Request.Method = HttpMethods.Post;
            c.Request.Path = "/weather";
        });
        Assert.True(context.Response.StatusCode == (int)HttpStatusCode.Unauthorized);
    }
}
