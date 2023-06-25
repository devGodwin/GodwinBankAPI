namespace GodwinBankAPI.Redis;

public class RedisConfig
{
    public string BaseUrl { get; set; } = string.Empty;
    public int Database { get; set; }
    public int DataExpiryDays { get; set; }
}