using System.ComponentModel.DataAnnotations;

namespace GodwinBankAPI.Model;

public class ResetPasswordRequestModel
{
    [Required(AllowEmptyStrings = false)]
    public string PasswordResetCode { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
    [Required,Compare("Password")]
    public string ConfirmPassword { get; set; }
}