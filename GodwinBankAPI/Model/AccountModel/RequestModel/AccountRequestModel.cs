using System.ComponentModel.DataAnnotations;
using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.Helper;

namespace GodwinBankAPI.Model.AccountModel.RequestModel;

public class AccountRequestModel
{
    [Required(AllowEmptyStrings = false)]
    public string UserGhCardId { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string FirstName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string LastName { get; set; }
    [Required,EmailAddress]
    public string EmailAddress { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string DateOfBirth { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Address { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Contact { get; set; }
    [Required]
    public decimal AccountBalance { get; set; }
    [Required]
    [RegularExpression("Current|Savings")]
    public string AccountType { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
    [Required,Compare("Password")] 
    public string ConfirmPassword { get; set; }
    
}


