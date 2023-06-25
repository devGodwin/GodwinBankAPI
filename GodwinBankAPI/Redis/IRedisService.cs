using GodwinBankAPI.Model;

namespace GodwinBankAPI.Redis;

public interface IRedisService
{
    Task<bool> CachedAccountAsync(CachedAccount cachedAccount);
    Task<CachedAccount> GetAccountAsync(string accountNumber);
    Task<bool> DeleteAccountAsync(string accountNumber);
}