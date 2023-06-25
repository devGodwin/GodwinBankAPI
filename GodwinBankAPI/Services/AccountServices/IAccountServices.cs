using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.Model.AccountModel.RequestModel;
using GodwinBankAPI.Model.AccountModel.ResponseModel;
using GodwinBankAPI.Model.Filters;
using GodwinBankAPI.Model.Response;
using GodwinBankAPI.Model.TransactionModel.RequestModel;
using GodwinBankAPI.Model.TransactionModel.ResponseModel;

namespace GodwinBankAPI.Services.AccountServices;

public interface IAccountServices
{
    Task<BaseResponse<PaginatedResponse<AccountResponseModel>>> GetAccountsAsync(AccountFilter accountFilter);
    Task<BaseResponse<AccountResponseModel>> GetAccountByAccountNumberAsync(string accountNumber);
     Task<BaseResponse<AccountResponseModel>> UpdateAccountAsync(string accountNumber, AccountUpdateModel updateModel);
     Task<BaseResponse<EmptyResponse>> DeleteAccountAsync(string accountNumber);
}