using System.ComponentModel.DataAnnotations;

namespace GodwinBankAPI.Model.AccountModel.RequestModel;

public class AccountLoginRequest
{
    [Required,EmailAddress]
    public string EmailAddress { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
}