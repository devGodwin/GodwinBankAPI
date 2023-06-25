using System.ComponentModel.DataAnnotations;
using GodwinBankAPI.Data.AccountData;

namespace GodwinBankAPI.Model.AccountModel.RequestModel;

public class AccountUpdateModel
{
    [Required(AllowEmptyStrings = false)]
    public string FirstName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string LastName { get; set; }
    [Required,EmailAddress]
    public string EmailAddress { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Address { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Contact { get; set; }

}