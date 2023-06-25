using GodwinBankAPI.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GodwinBankAPI.Redis;

public class RedisService:IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisService> _logger;
    private readonly RedisConfig _redisConfig;

    public RedisService(IConnectionMultiplexer connectionMultiplexer,ILogger<RedisService>logger, IOptions<RedisConfig>redisConfig)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
        _redisConfig = redisConfig.Value;
    }
    public async Task<bool> CachedAccountAsync(CachedAccount cachedAccount)
    {
        try
        {
            string accountKey = RedisConstants.GetAccountKeyByAccountNumber(cachedAccount.AccountNumber);
            var cachedSuccessfully = await _connectionMultiplexer.GetDatabase().StringSetAsync(
                key: accountKey,
                value: JsonConvert.SerializeObject(cachedAccount),
                TimeSpan.FromDays(_redisConfig.DataExpiryDays));

            return cachedSuccessfully;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured caching account by accountKey:{accountKey}",
                JsonConvert.SerializeObject(cachedAccount,Formatting.Indented));
            return false;
        }
    }

    public async Task<CachedAccount> GetAccountAsync(string accountNumber)
    {
        try
        {
            string accountKey = RedisConstants.GetAccountKeyByAccountNumber(accountNumber);
            var accountExist = await _connectionMultiplexer.GetDatabase().KeyExistsAsync(accountKey);
            if (accountExist)
            {
                var accountValue = await _connectionMultiplexer.GetDatabase().StringGetAsync(accountKey);
                
                return JsonSerializer.Deserialize<CachedAccount>(accountValue);
            }

            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting account by accountKey:{accountKey}",accountNumber);
            return null;
        }
    }

    public async Task<bool> DeleteAccountAsync(string accountNumber)
    {
        try
        {
            string accountKey = RedisConstants.GetAccountKeyByAccountNumber(accountNumber);
            var accountExist = await _connectionMultiplexer.GetDatabase().KeyExistsAsync(accountKey);
            if (accountExist)
            {
                var deletedSuccessfully = await _connectionMultiplexer.GetDatabase().KeyDeleteAsync(accountKey);

                return deletedSuccessfully;
            }

            return false;
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured deleting account by accountKey:{accountKey}",accountNumber);
           return false;
        }
    }
}