namespace Weather.Api.RateLimit;

[AttributeUsage(AttributeTargets.Method)]
public class RateLimiterAttr : Attribute
{
    public int TimeWindow { get; set; }
    public int MaxRequests { get; set; }
}
