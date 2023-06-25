using System.ComponentModel.DataAnnotations;
using GodwinBankAPI.Helper;

namespace GodwinBankAPI.Data.AccountData;

public class Account
{
    [Key] public string AccountNumber { get; set; } = AccountNumberGenerator.AccountNumber();
    public string UserGhCardId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AccountName { get; set; }
    public string EmailAddress { get; set; }
    public string DateOfBirth { get; set; }
    public string Address { get; set; }
    public string Contact { get; set; }
    public string AccountType { get; set; }
    public decimal AccountBalance { get; set; }
    public string AccountStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string PasswordResetCode { get; set; }
    public DateTime ResetCodeExpiry { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
}

