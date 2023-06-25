using System.ComponentModel.DataAnnotations;
using GodwinBankAPI.Data.TransactionData;

namespace GodwinBankAPI.Model.TransactionModel.RequestModel;

public class TransactionRequestModel
{
   
    [RegularExpression("Deposit|Withdrawal|Transfer")]
    public string TransactionType { get; set; }
    public decimal Amount { get; set; }
    public string SourceAccountNumber  { get; set; }
    public string DestinationAccountNumber  { get; set; }
    public string Description { get; set; }
    public string Password { get; set; }
}
