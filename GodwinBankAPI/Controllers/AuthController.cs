using GodwinBankAPI.Model;
using GodwinBankAPI.Model.AccountModel.RequestModel;
using GodwinBankAPI.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;

namespace GodwinBankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController:ControllerBase
{
    private readonly IAuthServices _authServices;

    public AuthController(IAuthServices authServices)
    {
        _authServices = authServices;
    }
    
    /// <summary>
    /// Create account
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody]AccountRequestModel requestModel)
    {
        var response = await _authServices.CreateAccountAsync(requestModel);
        
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Login account
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAccount([FromBody] AccountLoginRequest loginRequest)
    {
        var response = await _authServices.LoginAccountAsync(loginRequest);
        
        return StatusCode(response.Code, response);
    }
    
    /// <summary>
    /// Forgot password?
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("forgot_password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var response = await _authServices.ForgotPasswordAsync(request);
        
        return StatusCode( response.Code,response);
    }
    
    /// <summary>
    /// Reset password
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost("reset_password")]
    public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordRequestModel requestModel)
    {
        var response = await _authServices.ResetPasswordAsync(requestModel);
        
        return StatusCode( response.Code,response);
    }
}