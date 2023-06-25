using AutoMapper;
using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.Data.TransactionData;
using GodwinBankAPI.Helper;
using GodwinBankAPI.Model.Response;
using GodwinBankAPI.Model.TransactionModel.RequestModel;
using GodwinBankAPI.Model.TransactionModel.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GodwinBankAPI.Services.TransactionServices;

public class TransactionServices:ITransactionServices
{
    private readonly AccountContext _accountContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TransactionServices> _logger;

    public TransactionServices(AccountContext accountContext, IMapper mapper,ILogger<TransactionServices> logger)
    {
        _accountContext = accountContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<TransactionResponseModel>> DepositAsync(TransactionRequestModel requestModel)
    {
        try
        {
            var sourceAccount = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber.Equals(requestModel.SourceAccountNumber));
            var destinationAccount = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber.Equals(requestModel.DestinationAccountNumber));

           if (sourceAccount == null || destinationAccount == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<TransactionResponseModel>("Account not found");
            }
           
           if (sourceAccount.AccountStatus != "Active" || destinationAccount.AccountStatus != "Active")
            {
                return CommonResponses.ErrorResponse.ForbidErrorResponse<TransactionResponseModel>("Account is not active");
            }

            if (!Authentication.VerifyPasswordHash(requestModel.Password ,sourceAccount.PasswordHash,sourceAccount.PasswordSalt))
            {
                return CommonResponses.ErrorResponse.ForbidErrorResponse<TransactionResponseModel>("Password is incorrect");
            }

            sourceAccount.AccountBalance += requestModel.Amount;
            destinationAccount.AccountBalance -= requestModel.Amount;

            _accountContext.Accounts.Update(sourceAccount);
            _accountContext.Accounts.Update(destinationAccount);
            
            var rows = await _accountContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<TransactionResponseModel>("Transaction failed");
            }

            Transaction transaction = new Transaction();
            
            if (rows > 0)
            {
                transaction.TransactionStatus = "Success";
            }
            
            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<TransactionResponseModel>(sourceAccount),"Success");
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured making deposit\n{requestModel}", JsonConvert.SerializeObject(requestModel,Formatting.Indented));

           return CommonResponses.ErrorResponse.InternalServerErrorResponse<TransactionResponseModel>();
        }
    }

    public async Task<BaseResponse<TransactionResponseModel>> WithdrawAsync(TransactionRequestModel requestModel)
    {
        try
        {
            var sourceAccount = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber.Equals(requestModel.SourceAccountNumber));
            var destinationAccount = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber.Equals(requestModel.DestinationAccountNumber));

            if (sourceAccount == null || destinationAccount == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<TransactionResponseModel>("Account not found");
            }
            
            if (sourceAccount.AccountStatus != "Active" || destinationAccount.AccountStatus != "Active")
            {
                return CommonResponses.ErrorResponse.ForbidErrorResponse<TransactionResponseModel>("Account is not active");
            }
            
            if (!Authentication.VerifyPasswordHash(requestModel.Password,sourceAccount.PasswordHash,sourceAccount.PasswordSalt))
            {
                return CommonResponses.ErrorResponse.ForbidErrorResponse<TransactionResponseModel>("Password is incorrect");
            }

            sourceAccount.AccountBalance -= requestModel.Amount;
            destinationAccount.AccountBalance += requestModel.Amount;
            
            _accountContext.Accounts.Update(sourceAccount);
            _accountContext.Accounts.Update(destinationAccount);
            
            var rows = await _accountContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<TransactionResponseModel>("Transaction failed");
            }
            
            Transaction transaction = new Transaction();
            
            if (rows > 0)
            {
                transaction.TransactionStatus = "Success";
            }

            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<TransactionResponseModel>(destinationAccount),"Success");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured making withdrawal\n{requestModel}", JsonConvert.SerializeObject(requestModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<TransactionResponseModel>();
        }
    }

    public async Task<BaseResponse<TransactionResponseModel>> TransferAsync(TransactionRequestModel requestModel)
    {
        try
        {
            var sourceAccount = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber.Equals(requestModel.SourceAccountNumber));
            var destinationAccount = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber.Equals(requestModel.DestinationAccountNumber));

            if (sourceAccount == null || destinationAccount == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<TransactionResponseModel>("Account not found");
            }
            
            if (sourceAccount.AccountStatus != "Active" || destinationAccount.AccountStatus != "Active")
            {
                return CommonResponses.ErrorResponse.ForbidErrorResponse<TransactionResponseModel>("Account is not active");
            }
            
            if (!Authentication.VerifyPasswordHash(requestModel.Password,sourceAccount.PasswordHash,sourceAccount.PasswordSalt))
            {
                return CommonResponses.ErrorResponse.ForbidErrorResponse<TransactionResponseModel>("Password is incorrect");
            }

            if (sourceAccount.AccountBalance < requestModel.Amount)
            {
                CommonResponses.ErrorResponse.ForbidErrorResponse<TransactionResponseModel>("Insufficient balance");
            }

            sourceAccount.AccountBalance -= requestModel.Amount;
            destinationAccount.AccountBalance += requestModel.Amount;

            _accountContext.Accounts.Update(sourceAccount);
            _accountContext.Accounts.Update(destinationAccount);
            
            var rows = await _accountContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<TransactionResponseModel>("Transaction failed");
            }
            
            Transaction transaction = new Transaction();
            
            if (rows > 0)
            {
                transaction.TransactionStatus = "Success";
            }

            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<TransactionResponseModel>(destinationAccount), "Success");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured making transfer\n{requestModel}", JsonConvert.SerializeObject(requestModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<TransactionResponseModel>();
        }
    }
    
}