using System.Net;
using AutoFixture;
using AutoMapper;
using GodwinBankAPI.Controllers;
using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.Model;
using GodwinBankAPI.Model.AccountModel.RequestModel;
using GodwinBankAPI.Model.AccountModel.ResponseModel;
using GodwinBankAPI.Model.Response;
using GodwinBankAPI.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestBank.UnitTest.TestSetup;
using Xunit;

namespace TestBank.UnitTest;

public class AuthControllerTest:IClassFixture<TestFixture>
{
    private readonly AuthController _authController;
    private readonly Mock<IAuthServices> _authServiceMock = new Mock<IAuthServices>();
    private readonly Fixture _fixture = new Fixture();

    public AuthControllerTest(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<AuthController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();
        _authController = new AuthController(_authServiceMock.Object);
    }
    
    [Fact]
    public async Task Create_New_Account_If_Exist_Return_Conflict()
    {
        // Arrange
        var account = _fixture.Create<Account>();
        _authServiceMock.Setup(repos => repos.CreateAccountAsync(It.IsAny<AccountRequestModel>()))
            .ReturnsAsync(new BaseResponse<AccountResponseModel>()
            {
                Code = (int)HttpStatusCode.Conflict,
                Data = new AccountResponseModel(),
                Message = It.IsAny<string>()
            });

        // Act
        var result = await _authController.CreateAccount(new AccountRequestModel()
        {
           AccountBalance = account.AccountBalance,
           Address = account.Address,
           Contact = account.Contact,
           DateOfBirth = account.DateOfBirth,
           Password = It.IsAny<string>(),
           ConfirmPassword = It.IsAny<string>(),
           EmailAddress = account.EmailAddress,
           FirstName = account.FirstName,
           LastName = account.LastName
        }) as ObjectResult;

        // Assert
        Assert.Equal(409, result?.StatusCode);
    }
    
    [Fact]
    public async Task Create_New_Account_Return_Ok()
    {
        // Arrange
        var account = _fixture.Create<Account>();

        _authServiceMock.Setup(repos => repos.CreateAccountAsync(It.IsAny<AccountRequestModel>()))
            .ReturnsAsync(new BaseResponse<AccountResponseModel>()
            {
                Code = (int)HttpStatusCode.Created,
                Data = new AccountResponseModel(),
                Message = It.IsAny<string>()

            });

        // Act
        var result = await _authController.CreateAccount(new AccountRequestModel()
        {
            AccountBalance = account.AccountBalance,
            Address = account.Address,
            Contact = account.Contact,
            DateOfBirth = account.DateOfBirth,
            Password = It.IsAny<string>(),
            ConfirmPassword = It.IsAny<string>(),
            EmailAddress = account.EmailAddress,
            FirstName = account.FirstName,
            LastName = account.LastName
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(201,result?.StatusCode);
    }
    
    [Fact]
    public async Task Login_Account_Return_Ok()
    {
        // Arrange
        var account = _fixture.Create<Account>();
        
        _authServiceMock.Setup(repos => repos.LoginAccountAsync(It.IsAny<AccountLoginRequest>()))
            .ReturnsAsync(new BaseResponse<TokenResponse>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new TokenResponse(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _authController.LoginAccount(new AccountLoginRequest()
        {
            EmailAddress = account.EmailAddress,
            Password = It.IsAny<string>()
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Forgot_Password_Return_Ok()
    {
        // Arrange
        _authServiceMock.Setup(repos => repos.ForgotPasswordAsync(It.IsAny<ForgotPasswordRequest>()))
            .ReturnsAsync(new BaseResponse<EmptyResponse>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new EmptyResponse(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _authController.ForgotPassword(It.IsAny<ForgotPasswordRequest>()) as ObjectResult;
  
        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Reset_Password_Return_Ok()
    {
        // Arrange
        var account = _fixture.Create<Account>();
        
        _authServiceMock.Setup(repos => repos.ResetPasswordAsync(It.IsAny<ResetPasswordRequestModel>()))
            .ReturnsAsync(new BaseResponse<AccountResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new AccountResponseModel(),
                Message = It.IsAny<string>()
            });
        
        // Act
        var result = await _authController.ResetPassword(new ResetPasswordRequestModel()
        {
            PasswordResetCode = account.PasswordResetCode,
            Password = It.IsAny<string>(),
            ConfirmPassword = It.IsAny<string>()
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
}