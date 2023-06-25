using GodwinBankAPI.Data.AccountData;

namespace GodwinBankAPI.Model.AccountModel.ResponseModel;

public class AccountResponseModel
{ 
    public string UserGhCardId { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AccountName { get; set; }
    public string EmailAddress { get; set; }
    public string DateOfBirth { get; set; }
    public string Address { get; set; } 
    public string Contact { get; set; }
    public string AccountNumber { get; set; }  
    public string AccountType { get; set; }
    public decimal AccountBalance { get; set; }
    public string AccountStatus { get; set; }
    public DateTime CreatedAt { get; set; } 
    
}