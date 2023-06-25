using GodwinBankAPI.Model;
using GodwinBankAPI.Model.AccountModel.RequestModel;
using GodwinBankAPI.Model.AccountModel.ResponseModel;
using GodwinBankAPI.Model.Response;

namespace GodwinBankAPI.Services.AuthServices;

public interface IAuthServices
{
    Task<BaseResponse<AccountResponseModel>> CreateAccountAsync(AccountRequestModel accountRequestModel);
    Task<BaseResponse<TokenResponse>> LoginAccountAsync(AccountLoginRequest loginRequest);
    Task<BaseResponse<EmptyResponse>> ForgotPasswordAsync(ForgotPasswordRequest emailAddress);
    Task<BaseResponse<AccountResponseModel>> ResetPasswordAsync(ResetPasswordRequestModel requestModel);
}