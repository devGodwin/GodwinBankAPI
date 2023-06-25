namespace GodwinBankAPI.Model.Response;

public class TokenResponse
{
    public string BearerToken { get; set; }
    public int? Expiry { get; set; }
}