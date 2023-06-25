using System.Net;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.ElasticSearch;
using GodwinBankAPI.Model.AccountModel.RequestModel;
using GodwinBankAPI.Model.AccountModel.ResponseModel;
using GodwinBankAPI.Model.Filters;
using GodwinBankAPI.Model.Response;
using GodwinBankAPI.Redis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GodwinBankAPI.Services.AccountServices;

public class AccountServices:IAccountServices
{
    private readonly AccountContext _accountContext;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountServices> _logger;
    private readonly IRedisService _redisService;
    private readonly IElasticSearchService _elasticSearchService;

    public AccountServices(AccountContext accountContext,IMapper mapper,ILogger<AccountServices>logger,
        IRedisService redisService,IElasticSearchService elasticSearchService)
    {
        _accountContext = accountContext;
        _mapper = mapper;
        _logger = logger;
        _redisService = redisService;
        _elasticSearchService = elasticSearchService;
    }
    public async Task<BaseResponse<PaginatedResponse<AccountResponseModel>>> GetAccountsAsync(
        AccountFilter accountFilter)
    {
        try
        {
            var accountQueryable = _accountContext.Accounts.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(accountFilter.UserGhCardId))
        {
            accountQueryable = accountQueryable.Where(x => x.UserGhCardId.Equals(accountFilter.UserGhCardId));
        }

        if (!string.IsNullOrEmpty(accountFilter.FirstName))
        {
            accountQueryable = accountQueryable.Where(x => x.FirstName.Equals(accountFilter.FirstName));
        }

        if (!string.IsNullOrEmpty(accountFilter.LastName))
        {
            accountQueryable = accountQueryable.Where(x => x.LastName.Equals(accountFilter.LastName));
        }
        if (!string.IsNullOrEmpty(accountFilter.AccountName))
        {
            accountQueryable = accountQueryable.Where(x => x.AccountName.Equals(accountFilter.AccountName));
        }
        
        if (!string.IsNullOrEmpty(accountFilter.EmailAddress))
        {
            accountQueryable = accountQueryable.Where(x => x.EmailAddress.Equals(accountFilter.EmailAddress));
        }
        
        if (!string.IsNullOrEmpty(accountFilter.DateOfBirth))
        {
            accountQueryable = accountQueryable.Where(x => x.DateOfBirth.Equals(accountFilter.DateOfBirth));
        }
        
        if (!string.IsNullOrEmpty(accountFilter.Address))
        {
            accountQueryable = accountQueryable.Where(x => x.Address.Equals(accountFilter.Address));
        }
        
        if (!string.IsNullOrEmpty(accountFilter.Contact))
        {
            accountQueryable = accountQueryable.Where(x => x.Contact.Equals(accountFilter.Contact));
        }
        
        if (!string.IsNullOrEmpty(accountFilter.AccountNumber))
        {
            accountQueryable = accountQueryable.Where(x => x.AccountNumber.Equals(accountFilter.AccountNumber));
        }
        
        if (!string.IsNullOrEmpty(accountFilter.AccountType))
        {
            accountQueryable = accountQueryable.Where(x => x.AccountType.Equals(accountFilter.AccountType));
        }
        
        if (!string.IsNullOrEmpty(accountFilter.AccountStatus))
        {
            accountQueryable = accountQueryable.Where(x => x.AccountStatus.Equals(accountFilter.AccountStatus));
        }

        accountQueryable = "desc".Equals(accountFilter.OrderBy, StringComparison.OrdinalIgnoreCase)
            ? accountQueryable.OrderByDescending(x => x.CreatedAt)
            : accountQueryable.OrderBy(x => x.CreatedAt);

        var paginatedResponse = await accountQueryable.ToPagedListAsync(accountFilter.CurrentPage-1, accountFilter.PageSize);

        return new BaseResponse<PaginatedResponse<AccountResponseModel>>()
        {
            Code = (int)HttpStatusCode.OK,
            Message = "Retrieved successfully ",
            Data = new PaginatedResponse<AccountResponseModel>()
            {
                CurrentPage = accountFilter.CurrentPage,
                PageSize = accountFilter.PageSize,
                TotalPages = paginatedResponse.TotalPages,
                TotalRecords = paginatedResponse.TotalCount,
                Data = paginatedResponse.Items.Select(x => _mapper.Map<AccountResponseModel>(x)).ToList()
            }
        };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting accounts\n{accountFilter}",
                JsonConvert.SerializeObject(accountFilter,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<PaginatedResponse<AccountResponseModel>>();
        }
    }

    public async Task<BaseResponse<AccountResponseModel>> GetAccountByAccountNumberAsync(string accountNumber)
    {
        try
        {
            var accountExist = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber.Equals(accountNumber));
            if (accountExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<AccountResponseModel>("Account not found");
            }

            await _redisService.GetAccountAsync(accountNumber);

            await _elasticSearchService.GetByIdAsync<AccountResponseModel>(accountNumber);

            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<AccountResponseModel>(accountExist));
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured getting account by accountNumber:{accountNumber}",accountNumber);
            
            return CommonResponses.ErrorResponse.InternalServerErrorResponse<AccountResponseModel>();
        }
    }

    public async Task<BaseResponse<AccountResponseModel>> UpdateAccountAsync(string accountNumber, AccountUpdateModel updateModel)
    {
        try
        {
            var accountExist = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber.Equals(accountNumber));
            if (accountExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<AccountResponseModel>("Account not found");
            }

            if (accountExist.AccountStatus != "Active")
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<AccountResponseModel>("Account is not active");
            }

            var newAccount = _mapper.Map(updateModel, accountExist);

            _accountContext.Accounts.Update(newAccount);
            var rows = await _accountContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<AccountResponseModel>();
            }

            await _elasticSearchService.UpdateAsync(accountExist.AccountNumber, newAccount);

            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<AccountResponseModel>(newAccount));
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating account\n{updateModel}", 
                JsonConvert.SerializeObject(updateModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<AccountResponseModel>();
        }
    }

    public async Task<BaseResponse<EmptyResponse>> DeleteAccountAsync(string accountNumber)
    {
        try
        {
            var accountExist = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber.Equals(accountNumber));
            if (accountExist == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>("Account not found");
            }

            _accountContext.Accounts.Remove(accountExist);

            var rows = await _accountContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<EmptyResponse>();
            }

            await _redisService.DeleteAccountAsync(accountNumber);

            await _elasticSearchService.DeleteAsync<EmptyResponse>(accountNumber);

            return CommonResponses.SuccessResponse.DeletedResponse();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured deleting account by accountNumber:{accountNumber}",accountNumber);

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<EmptyResponse>();
        }
    }
}