using GodwinBankAPI.Model.Response;
using GodwinBankAPI.Model.TransactionModel.RequestModel;
using GodwinBankAPI.Model.TransactionModel.ResponseModel;

namespace GodwinBankAPI.Services.TransactionServices;

public interface ITransactionServices
{
    Task<BaseResponse<TransactionResponseModel>> DepositAsync(TransactionRequestModel requestModel);
    Task<BaseResponse<TransactionResponseModel>> WithdrawAsync(TransactionRequestModel requestModel);
    Task<BaseResponse<TransactionResponseModel>> TransferAsync(TransactionRequestModel requestModel);
    
}