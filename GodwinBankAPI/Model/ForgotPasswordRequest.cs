using System.ComponentModel.DataAnnotations;

namespace GodwinBankAPI.Model;

public class ForgotPasswordRequest
{
    [Required(AllowEmptyStrings = false),EmailAddress]
    public string EmailAddress { get; set; }
}