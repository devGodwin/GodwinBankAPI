using System.Net;
using AutoFixture;
using AutoMapper;
using GodwinBankAPI.Controllers;
using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.Model.AccountModel.RequestModel;
using GodwinBankAPI.Model.AccountModel.ResponseModel;
using GodwinBankAPI.Model.Filters;
using GodwinBankAPI.Model.Response;
using GodwinBankAPI.Services.AccountServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestBank.UnitTest.TestSetup;
using Xunit;

namespace TestBank.UnitTest;

public class AccountControllerTest:IClassFixture<TestFixture>
{
    private readonly AccountController _accountController;
    private readonly Mock<IAccountServices> _accountServicesMock = new Mock<IAccountServices>();
    private readonly Fixture _fixture = new Fixture();

    public AccountControllerTest(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<AccountController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();

        _accountController = new AccountController(_accountServicesMock.Object);
    }

    [Fact]
    public async Task Filter_Account_Return_Ok()
    {
        // Arrange
        var account = _fixture.Create<PaginatedResponse<Account>>();
        _accountServicesMock.Setup(repos => repos.GetAccountsAsync(It.IsAny<AccountFilter>()))
            .ReturnsAsync(new BaseResponse<PaginatedResponse<AccountResponseModel>>()
            {
                Code  = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new PaginatedResponse<AccountResponseModel>()
                {
                    CurrentPage = account.CurrentPage,
                    PageSize = account.PageSize,
                    TotalRecords = account.TotalRecords,
                    TotalPages = account.TotalPages,
                    Data = new List<AccountResponseModel>()
                }
            });

        // Act
        var result = await _accountController.GetAccounts(It.IsAny<AccountFilter>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Get_Account_ById_Return_Ok()
    {
        // Arrange
        _accountServicesMock.Setup(repos => repos.GetAccountByAccountNumberAsync(It.IsAny<string>()))
            .ReturnsAsync(new BaseResponse<AccountResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new AccountResponseModel(),
                Message = It.IsAny<string>()
            });

        // Act
        var result = await _accountController.GetAccountById(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Account_If_Null_Return_NotFound()
    {
        // Arrange
        var account = _fixture.Create<Account>();
       
        _accountServicesMock.Setup(repos =>
                repos.UpdateAccountAsync(It.IsAny<string>(), It.IsAny<AccountUpdateModel>()))
            .ReturnsAsync(new BaseResponse<AccountResponseModel>()
            {
                Code = (int)HttpStatusCode.NotFound,
                Message = It.IsAny<string>(),
                Data = new AccountResponseModel()
            });

        // Act
        var result = await _accountController.UpdateAccount(It.IsAny<string>(),new AccountUpdateModel()
        {
            Address = account.Address,
            Contact = account.Contact,
            EmailAddress = account.EmailAddress,
            FirstName = account.FirstName,
            LastName = account.LastName,
        }) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Account_Return_Ok()
    {
        // Arrange
        var account = _fixture.Create<Account>();
       
        _accountServicesMock.Setup(repos =>
                repos.UpdateAccountAsync(It.IsAny<string>(), It.IsAny<AccountUpdateModel>()))
            .ReturnsAsync(new BaseResponse<AccountResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new AccountResponseModel()
            });

        // Act
        var result = await _accountController.UpdateAccount(It.IsAny<string>(),new AccountUpdateModel()
        {
            Address = account.Address,
            Contact = account.Contact,
            EmailAddress = account.EmailAddress,
            FirstName = account.FirstName,
            LastName = account.LastName,
        }) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Account_If_Null_Return_NotFound()
    {
        // Arrange
        var mockResponse = CommonResponses.ErrorResponse.NotFoundErrorResponse<EmptyResponse>(It.IsAny<string>());
        
        _accountServicesMock.Setup(repos => repos.DeleteAccountAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _accountController.DeleteAccount(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Account_Return_Ok()
    {
        // Arrange
        var mockResponse = CommonResponses.SuccessResponse.DeletedResponse();
        
        _accountServicesMock.Setup(repos => repos.DeleteAccountAsync(It.IsAny<string>()))
            .ReturnsAsync(mockResponse);

        // Act
        var result = (ObjectResult) await _accountController.DeleteAccount(It.IsAny<string>());

        // Assert
        Assert.Equal(200,result.StatusCode);
    }

}