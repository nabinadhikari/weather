using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Weather.Contracts;

namespace Weather.RequestRecordData;

public class RequestRecordService : IRequestRecordService
{
    private readonly IDistributedCache _cache;

    public RequestRecordService(IDistributedCache cache)
    {
        _cache = cache;
    }
    public async Task<RecordStatistics?> GetRequestKeyStatistics(string key)
    {
        var strRecord = await _cache.GetStringAsync(key);
        if (strRecord == null) return null;
        var record = JsonConvert.DeserializeObject<RecordStatistics>(strRecord);
        return record;
    }

    public async Task UpdateStatistics(string key, int maxRequest, double windowSeconds)
    {
        var record = new RecordStatistics();
        var savedRecord = await GetRequestKeyStatistics(key);
        if (savedRecord != null)
        {
            record.NumberOfSuccessfulRequest = savedRecord.NumberOfSuccessfulRequest + 1 > maxRequest ? maxRequest : savedRecord.NumberOfSuccessfulRequest + 1;
            record.LastSuccessfulResponseTime = DateTime.UtcNow;
        }
        else
        {
            record.NumberOfSuccessfulRequest = 1;
            record.LastSuccessfulResponseTime = DateTime.UtcNow;
        }
        await _cache.SetStringAsync(key,
            JsonConvert.SerializeObject(record),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(windowSeconds)
            });
    }
}
