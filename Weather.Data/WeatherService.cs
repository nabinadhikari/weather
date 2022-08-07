using Newtonsoft.Json;
using Weather.Contracts;
using Weather.Contracts.CONFIG;

namespace Weather.Data;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _http;
    private readonly WeatherServiceConfig _config;
    public WeatherService(WeatherServiceConfig config)
    {
        _config = config;
        _http = new HttpClient();
    }

    private string GetApiKey()
    {
        var random = new Random();
        var randomIndex = random.Next(_config.ApiKeys.Length);
        return _config.ApiKeys[randomIndex];
    }

    public async Task<ApiResult> GetWeatherForCityAsync(string city, string country)
    {
        var url = $"{_config.ApiUrl}?q={city},{country}&appid={GetApiKey()}";
        try
        {
            var response = await _http.GetAsync(url);
            if (response != null && response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(data))
                {
                    var weatherData = JsonConvert.DeserializeObject<OpenWeatherResponse>(data);
                    if (weatherData != null && weatherData.weather.Count > 0)
                    {
                        return new ApiResult
                        {
                            ErrorNo = 0,
                            Message = weatherData.weather[0].description,
                        };
                    }
                }
            }
            else if (response != null)
            {
                return new ApiResult
                {
                    ErrorNo = (int)response.StatusCode,
                    Message = response.ReasonPhrase,
                };
            }
        }
        catch (Exception ex)
        {
            return new ApiResult
            {
                ErrorNo = 1,
                Message = ex.Message,
            };
        }
        return new ApiResult
        {
            ErrorNo = 1,
            Message = "Weather detail not available. Please try again later.",
        };
    }
}
