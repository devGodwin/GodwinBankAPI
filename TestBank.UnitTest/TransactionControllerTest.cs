using System.Net;
using AutoFixture;
using AutoMapper;
using GodwinBankAPI.Controllers;
using GodwinBankAPI.Model.Response;
using GodwinBankAPI.Model.TransactionModel.RequestModel;
using GodwinBankAPI.Model.TransactionModel.ResponseModel;
using GodwinBankAPI.Services.TransactionServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestBank.UnitTest.TestSetup;
using Xunit;

namespace TestBank.UnitTest;

public class TransactionControllerTest:IClassFixture<TestFixture>
{
    private readonly TransactionController _transactionController;
    private readonly Mock<ITransactionServices> _transactionServicesMock = new Mock<ITransactionServices>();
    private readonly Fixture _fixture = new Fixture();

    public TransactionControllerTest(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<TransactionController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();

        _transactionController = new TransactionController(_transactionServicesMock.Object);
    }
    
    [Fact]
    public async Task Make_Deposit_If_Null_Return_NotFound()
    {
        // Arrange
        _transactionServicesMock.Setup(repos => repos.DepositAsync(It.IsAny<TransactionRequestModel>()))
            .ReturnsAsync(new BaseResponse<TransactionResponseModel>()
            {
                Code  = (int)HttpStatusCode.NotFound,
                Message = It.IsAny<string>(),
                Data = new TransactionResponseModel()
            });

        // Act
        var result = await _transactionController.MakeDeposit(It.IsAny<TransactionRequestModel>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }

    [Fact]
    public async Task Make_Deposit_Return_Ok()
    {
        // Arrange
        _transactionServicesMock.Setup(repos => repos.DepositAsync(It.IsAny<TransactionRequestModel>()))
            .ReturnsAsync(new BaseResponse<TransactionResponseModel>()
            {
                Code  = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new TransactionResponseModel()
            });

        // Act
        var result = await _transactionController.MakeDeposit(It.IsAny<TransactionRequestModel>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Make_Withdrawal_If_Null_Return_NotFound()
    {
        // Arrange
        _transactionServicesMock.Setup(repos => repos.DepositAsync(It.IsAny<TransactionRequestModel>()))
            .ReturnsAsync(new BaseResponse<TransactionResponseModel>()
            {
                Code  = (int)HttpStatusCode.NotFound,
                Message = It.IsAny<string>(),
                Data = new TransactionResponseModel()
            });

        // Act
        var result = await _transactionController.MakeDeposit(It.IsAny<TransactionRequestModel>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Make_Withdrawal_Return_Ok()
    {
        // Arrange
        _transactionServicesMock.Setup(repos => repos.WithdrawAsync(It.IsAny<TransactionRequestModel>()))
            .ReturnsAsync(new BaseResponse<TransactionResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Data = new TransactionResponseModel(),
                Message = It.IsAny<string>()
            });

        // Act
        var result = await _transactionController.MakeWithdrawal(It.IsAny<TransactionRequestModel>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Make_Transfer_If_Null_Return_NotFound()
    {
        // Arrange
        _transactionServicesMock.Setup(repos => repos.DepositAsync(It.IsAny<TransactionRequestModel>()))
            .ReturnsAsync(new BaseResponse<TransactionResponseModel>()
            {
                Code  = (int)HttpStatusCode.NotFound,
                Message = It.IsAny<string>(),
                Data = new TransactionResponseModel()
            });

        // Act
        var result = await _transactionController.MakeDeposit(It.IsAny<TransactionRequestModel>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Make_Transfer_Return_Ok()
    {
        // Arrange
        _transactionServicesMock.Setup(repos =>
                repos.TransferAsync(It.IsAny<TransactionRequestModel>()))
            .ReturnsAsync(new BaseResponse<TransactionResponseModel>()
            {
                Code = (int)HttpStatusCode.OK,
                Message = It.IsAny<string>(),
                Data = new TransactionResponseModel()
            });

        // Act
        var result = await _transactionController.MakeTransfer(It.IsAny<TransactionRequestModel>()) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }

}