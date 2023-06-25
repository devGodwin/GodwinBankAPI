using GodwinBankAPI.Model.TransactionModel.RequestModel;
using GodwinBankAPI.Services.TransactionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GodwinBankAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
//[Authorize(AuthenticationSchemes = "Bearer")]
public class TransactionController:ControllerBase
{
    private readonly ITransactionServices _transactionServices;

    public TransactionController(ITransactionServices transactionServices)
    {
        _transactionServices = transactionServices;
    }

    /// <summary>
    /// Make a deposit
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost("deposit")]
    public async Task<IActionResult> MakeDeposit([FromBody]TransactionRequestModel requestModel)
    {
        var response = await _transactionServices.DepositAsync(requestModel);

        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Make a withdrawal
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost("withdraw")]
    public async Task<IActionResult> MakeWithdrawal([FromBody]TransactionRequestModel requestModel)
    {
        var response = await _transactionServices.WithdrawAsync(requestModel);

        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Make a transfer
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost("transfer")]
    public async Task<IActionResult> MakeTransfer([FromBody]TransactionRequestModel requestModel)
    {
        var response = await _transactionServices.TransferAsync(requestModel);

        return StatusCode(response.Code, response);
    }
}