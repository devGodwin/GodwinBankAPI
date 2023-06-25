namespace GodwinBankAPI.Redis;

public static class RedisConstants
{
    private const string AccountKeyByAccountNumber = "GodwinBankAPI:accountNumber:{accountNumber}";

    public static string GetAccountKeyByAccountNumber(string accountNumber) =>
        AccountKeyByAccountNumber.Replace("{accountNumber}", accountNumber);
}