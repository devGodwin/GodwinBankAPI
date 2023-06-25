using System.ComponentModel.DataAnnotations;
using GodwinBankAPI.Data.AccountData;

namespace GodwinBankAPI.Data.TransactionData;

public class Transaction
{
    [Key]
    public string TransactionId { get; set; } = Guid.NewGuid().ToString("N");
    public string TransactionType { get; set; }
    public decimal AccountBalance { get; set; }
    public string SourceAccountNumber { get; set; }            
    public string DestinationAccountNumber  { get; set; }  
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public string TransactionStatus { get; set; }       
}

