﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Weather.Contracts;

namespace Weather.Data;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _http;
    public WeatherService()
    {
        _http = new HttpClient();
    }
    public async Task<ApiResult> GetWeatherForCityAsync(string city, string country)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city},{country}&appid=8b7535b42fe1c551f18028f64e8688f7";
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