using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Weather.Api.Controllers;
using Weather.Contracts;
using Weather.Data;
using Xunit;

namespace Weather.Api.Tests;

public class WeatherControllerTests
{
    [Fact]
    public void UnitOfWork_StateUnderTest_ExpectedBehaviour()
    {

    }

    [Fact]
    public async Task GetWeatherDetails_WithoutCityOrCountry_ReturnsBadRequest()
    {
        // Arrange
        var wsStub = new Mock<IWeatherService>();
        var loggerStub = new Mock<ILogger<WeatherController>>();
        var controller = new WeatherController(loggerStub.Object, wsStub.Object);
        // Act
        var result = await controller.GetWeatherDetails(new WeatherRequestDto { City = "", Country = "" });
        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task GetWeatherDetails_WithUnexistingCityOrCountry_ReturnsNotFound()
    {
        // Arrange
        var wsStub = new Mock<IWeatherService>();
        wsStub.Setup(ws => ws.GetWeatherForCityAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ApiResult { ErrorNo = 404, Message = "Not Found" });
        var loggerStub = new Mock<ILogger<WeatherController>>();
        var controller = new WeatherController(loggerStub.Object, wsStub.Object);
        var dto = new WeatherRequestDto { City = "asdfasqwer", Country = "asdfasdf" };
        // Act
        var result = await controller.GetWeatherDetails(dto);
        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetWeatherDetails_WithValidCityOrCountry_ReturnsResultOk()
    {
        // Arrange
        var wsStub = new Mock<IWeatherService>();
        wsStub.Setup(ws => ws.GetWeatherForCityAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ApiResult { ErrorNo = 0, Message = "scattered clouds" });
        var loggerStub = new Mock<ILogger<WeatherController>>();
        var controller = new WeatherController(loggerStub.Object, wsStub.Object);
        var dto = new WeatherRequestDto { City = "london", Country = "uk" };
        // Act
        var result = await controller.GetWeatherDetails(dto);
        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }
}
