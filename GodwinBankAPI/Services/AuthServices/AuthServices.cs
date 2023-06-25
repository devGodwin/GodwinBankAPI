using AutoMapper;
using GodwinBankAPI.Configurations;
using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.ElasticSearch;
using GodwinBankAPI.Helper;
using GodwinBankAPI.Model;
using GodwinBankAPI.Model.AccountModel.RequestModel;
using GodwinBankAPI.Model.AccountModel.ResponseModel;
using GodwinBankAPI.Model.Response;
using GodwinBankAPI.Redis;
using GodwinBankAPI.Services.EmailServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GodwinBankAPI.Services.AuthServices;
public class AuthServices:IAuthServices
{
    private readonly AccountContext _accountContext;
    private readonly ILogger<AuthServices> _logger;
    private readonly IMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly IElasticSearchService _elasticSearchService;
    private readonly IEmailService _emailService;
    private readonly BearerTokenConfig _bearerTokenConfig;

    public AuthServices(AccountContext accountContext,ILogger<AuthServices>logger,IMapper mapper, 
        IOptions<BearerTokenConfig> bearerTokenConfig,IRedisService redisService,IElasticSearchService elasticSearchService,
        IEmailService emailService)
    {
        _accountContext = accountContext;
        _logger = logger;
        _mapper = mapper;
        _redisService = redisService;
        _elasticSearchService = elasticSearchService;
        _emailService = emailService;
        _bearerTokenConfig = bearerTokenConfig.Value;
    }
    public async Task<BaseResponse<AccountResponseModel>> CreateAccountAsync(AccountRequestModel accountRequestModel)
    {
        try
        {
            // Prevent duplicate account number
            var accountExist = await _accountContext.Accounts.AnyAsync(x => x.AccountNumber.Equals(AccountNumberGenerator.AccountNumber()));
            if (accountExist)
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<AccountResponseModel>("Something bad happened, try again");
            }

            // Limit number of accounts to three  
            var accountLimit = await _accountContext.Accounts.CountAsync(x => x.UserGhCardId.Equals(accountRequestModel.UserGhCardId));
            if (accountLimit > 2)
            {
                return CommonResponses.ErrorResponse.ForbidErrorResponse<AccountResponseModel>("User has reached the maximum allowed number of accounts");
            }

            Authentication.CreatePasswordHash(accountRequestModel.Password,out byte[] passwordHash, out byte[] passwordSalt);
            
            var newAccount = _mapper.Map<Account>(accountRequestModel);
            
            newAccount.PasswordHash = passwordHash;
            newAccount.PasswordSalt = passwordSalt;

            newAccount.AccountName = newAccount.FirstName +" "+ newAccount.LastName;
            newAccount.AccountStatus = "Active";
            
            await _accountContext.Accounts.AddAsync(newAccount);
            var rows = await _accountContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<AccountResponseModel>();
            }

            await _redisService.CachedAccountAsync(_mapper.Map<CachedAccount>(newAccount));

            await _elasticSearchService.AddAsync(newAccount);

            return CommonResponses.SuccessResponse.CreatedResponse(_mapper.Map<AccountResponseModel>(newAccount));
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured registering user\n{userRequestModel}",
                JsonConvert.SerializeObject(accountRequestModel,Formatting.Indented));

            return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<AccountResponseModel>();
        }
    }

    public async Task<BaseResponse<TokenResponse>> LoginAccountAsync(AccountLoginRequest loginRequest)
    {
        try
        {
            var account = await _accountContext.Accounts.FirstOrDefaultAsync(x=>x.EmailAddress.Equals(loginRequest.EmailAddress));

            if (!Authentication.VerifyPasswordHash(loginRequest.Password,account.PasswordHash,account.PasswordSalt))
            {
                return CommonResponses.ErrorResponse.ConflictErrorResponse<TokenResponse>("Password is incorrect");
            }

            AccountResponseModel accountResponseModel = new AccountResponseModel()
            {
                EmailAddress = loginRequest.EmailAddress
            };
            
            var token = TokenGenerator.GenerateToken(accountResponseModel, _bearerTokenConfig);

            return CommonResponses.SuccessResponse.OkResponse(token, "successful");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured logging in account \n{loginRequest}", 
                JsonConvert.SerializeObject(loginRequest,Formatting.Indented));

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<TokenResponse>();
        }
    }
 
    public async Task<BaseResponse<EmptyResponse>> ForgotPasswordAsync(ForgotPasswordRequest emailAddress)
    {
        try
        {
            var account = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.EmailAddress.Equals(emailAddress.EmailAddress));
            if (account == null)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>("Account not found");
            }

            Random random = new Random();
            var passwordResetCode = random.Next(100000, 900000).ToString();

            account.PasswordResetCode = passwordResetCode;
            account.ResetCodeExpiry = DateTime.UtcNow.AddHours(12);

            var rows = await _accountContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<EmptyResponse>();
            }
            
            await _emailService.SendEmail(); 
            
            return CommonResponses.SuccessResponse.OkResponse<EmptyResponse>("You may reset your password");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured saving PasswordResetCode:{emailAddress}",emailAddress);
            
            return CommonResponses.ErrorResponse.InternalServerErrorResponse<EmptyResponse>();
        }
    }

    public async Task<BaseResponse<AccountResponseModel>> ResetPasswordAsync(ResetPasswordRequestModel requestModel)
    {
        try
        {
            var account = await _accountContext.Accounts.FirstOrDefaultAsync(x => x.PasswordResetCode.Equals(requestModel.PasswordResetCode));
            if (account == null || account.ResetCodeExpiry < DateTime.UtcNow)
            {
                return CommonResponses.ErrorResponse.NotFoundErrorResponse<AccountResponseModel>("Account not found");
            }

            Authentication.CreatePasswordHash(requestModel.Password,out byte[] passwordHash,out byte[] passwordSalt);
            account.PasswordHash = passwordHash;
            account.PasswordSalt = passwordSalt;

            var rows = await _accountContext.SaveChangesAsync();
            if (rows < 1)
            {
                return CommonResponses.ErrorResponse.FailedDependencyErrorResponse<AccountResponseModel>();
            }
            
            return CommonResponses.SuccessResponse.OkResponse(_mapper.Map<AccountResponseModel>(account),"Password reset successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured resetting password by PasswordResetCode\n{requestModel}",
                JsonConvert.SerializeObject(requestModel,Formatting.Indented));
            
            return CommonResponses.ErrorResponse.InternalServerErrorResponse<AccountResponseModel>();
        }
    }
}