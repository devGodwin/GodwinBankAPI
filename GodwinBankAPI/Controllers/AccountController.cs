using GodwinBankAPI.Model.AccountModel.RequestModel;
using GodwinBankAPI.Model.Filters;
using GodwinBankAPI.Services.AccountServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GodwinBankAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
//[Authorize(AuthenticationSchemes = "Bearer")]
public class AccountController:ControllerBase
{
    private readonly IAccountServices _accountServices;

    public AccountController(IAccountServices accountServices)
    {
        _accountServices = accountServices;
    }

    /// <summary>
    /// Filter account
    /// </summary>
    /// <param name="accountFilter"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery]AccountFilter accountFilter)
    {
        var response = await _accountServices.GetAccountsAsync(accountFilter);
        
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Retrieve account 
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <returns></returns>
    [HttpGet("{accountNumber:required}")]
    public async Task<IActionResult> GetAccountById([FromRoute]string accountNumber)
    {
        var response = await _accountServices.GetAccountByAccountNumberAsync(accountNumber);

        return StatusCode(response.Code, response);
    }
    
    /// <summary>
    /// Update account
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="updateModel"></param>
    /// <returns></returns>
    [HttpPut("{accountNumber:required}")]
    public async Task<IActionResult> UpdateAccount([FromRoute]string accountNumber,[FromBody]AccountUpdateModel updateModel)
    {
        var response = await _accountServices.UpdateAccountAsync(accountNumber,updateModel);

        return StatusCode(response.Code, response);
    }
    
    /// <summary>
    /// Delete account
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <returns></returns>
    [HttpDelete("{accountNumber:required}")]
    public async Task<IActionResult> DeleteAccount([FromRoute]string accountNumber)
    {
        var response = await _accountServices.DeleteAccountAsync(accountNumber);

        return StatusCode(response.Code, response);
    }
    
}