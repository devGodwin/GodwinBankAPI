namespace GodwinBankAPI.Model.TransactionModel.ResponseModel;

public class TransactionResponseModel
{
    public string TransactionId { get; set; }
    public string TransactionType { get; set; }
    public decimal AccountBalance { get; set; } 
    public string SourceAccountNumber  { get; set; }            
    public string DestinationAccountNumber  { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public string TransactionStatus { get; set; }       
}