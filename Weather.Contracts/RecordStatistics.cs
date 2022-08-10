namespace Weather.Contracts;

public class RecordStatistics
{
    public DateTime LastSuccessfulResponseTime { get; set; }
    public int NumberOfSuccessfulRequest { get; set; }
}
