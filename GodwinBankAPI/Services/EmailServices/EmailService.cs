using GodwinBankAPI.Configurations;
using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.Model.Response;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace GodwinBankAPI.Services.EmailServices;

public class EmailService:IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly AccountContext _accountContext;
    private readonly EmailConfig _config;

    public EmailService(ILogger<EmailService> logger, IOptions<EmailConfig>config,AccountContext accountContext)
    {
        _logger = logger;
        _accountContext = accountContext;
        _config = config.Value;
    }
    public async Task<BaseResponse<EmptyResponse>> SendEmail()
    {
        try
        {
            var resetCode = await _accountContext.Accounts.FirstOrDefaultAsync();
            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.FromEmailUserName));
            email.To.Add(MailboxAddress.Parse(_config.ToEmailUserName));
            email.Subject = "Reset password";
            email.Body = new TextPart(TextFormat.Plain) { Text = $"Reset your password with this code: {resetCode.PasswordResetCode}"};

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.EmailHost,587,SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.ToEmailUserName,_config.EmailPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            
            return CommonResponses.SuccessResponse.OkResponse<EmptyResponse>("Successful");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured sending verification email");
            
            return CommonResponses.ErrorResponse.InternalServerErrorResponse<EmptyResponse>();
        }
    }
}