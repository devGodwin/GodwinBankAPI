using GodwinBankAPI.Data.AccountData;
using Microsoft.EntityFrameworkCore;

namespace GodwinBankAPI.Helper;

public static class AccountNumberGenerator
{
    public static string AccountNumber()
    {
        string bankCode = "123";
        string branchCode = "456";
        string fullAccountNumber = bankCode + branchCode + GenerateRandomAccountNumber();

        return fullAccountNumber;
    }

    public static string GenerateRandomAccountNumber()
    {
        Random random = new Random();
        int accountNumberLength = 7;

        string accountNumber = "";
        for (int i = 0; i < accountNumberLength; i++)
        {
            accountNumber += random.Next(0, 7).ToString();
        }
        
        return accountNumber;
        
    }
}