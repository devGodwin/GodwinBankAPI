using GodwinBankAPI.Model.Response;

namespace GodwinBankAPI.Services.EmailServices;

public interface IEmailService
{
    Task<BaseResponse<EmptyResponse>> SendEmail();
}