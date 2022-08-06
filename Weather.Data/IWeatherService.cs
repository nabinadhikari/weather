using Weather.Contracts;

namespace Weather.Data;

public interface IWeatherService
{
    Task<ApiResult> GetWeatherForCityAsync(string city, string country);
}
