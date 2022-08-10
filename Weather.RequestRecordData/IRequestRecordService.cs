using Weather.Contracts;

namespace Weather.RequestRecordData;

public interface IRequestRecordService
{
    Task<RecordStatistics?> GetRequestKeyStatistics(string key);
    Task UpdateStatistics(string key, int maxRequest, double windowSeconds);
}
