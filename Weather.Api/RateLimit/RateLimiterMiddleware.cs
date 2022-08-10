using System.Net;
using Weather.RequestRecordData;

namespace Weather.Api.RateLimit;

public class RateLimiterMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRequestRecordService _recordService;

    public RateLimiterMiddleware(RequestDelegate next, IRequestRecordService recordService)
    {
        _next = next;
        _recordService = recordService;
    }
    private string GetApiKey(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("x-api-key", out var apiKey)) return apiKey;
        return string.Empty;
    }

    private async Task<bool> IsValidApiKey(string apiKey)
    {
        return ConstantApiKeys.ApiKeys.Contains(apiKey);
    }


    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var decorator = endpoint?.Metadata.GetMetadata<RateLimiterAttr>();
        if (decorator is null)
        {
            await _next(context);
            return;
        }
        Console.WriteLine("Handle request limit");
        var apiKeyUsed = GetApiKey(context);
        if (string.IsNullOrEmpty(apiKeyUsed) || !await IsValidApiKey(apiKeyUsed))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        var keyStatistics = await _recordService.GetRequestKeyStatistics(apiKeyUsed);
        if(keyStatistics != null)
        {
            Console.WriteLine($"Time: {DateTime.UtcNow < keyStatistics.LastSuccessfulResponseTime.AddSeconds(decorator.TimeWindow)}");
            Console.WriteLine($"Number: {keyStatistics.NumberOfSuccessfulRequest == decorator.MaxRequests}");
        }

        if (keyStatistics != null &&
               DateTime.UtcNow < keyStatistics.LastSuccessfulResponseTime.AddSeconds(decorator.TimeWindow) &&
               keyStatistics.NumberOfSuccessfulRequest == decorator.MaxRequests)
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            return;
        }
        await _recordService.UpdateStatistics(apiKeyUsed, decorator.MaxRequests, decorator.TimeWindow);
        await _next(context);
    }
}

public static class RateLimiterMiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimiterMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimiterMiddleware>();
    }
}