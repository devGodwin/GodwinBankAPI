namespace GodwinBankAPI.Configurations;

public class EmailConfig
{
    public string EmailHost { get; set; } = string.Empty;
    public string ToEmailUserName { get; set; } = string.Empty;
    public string FromEmailUserName { get; set; } = string.Empty;
    public string EmailPassword { get; set; } = string.Empty;
}