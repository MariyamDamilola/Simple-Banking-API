using Microsoft.AspNetCore.Mvc;
using SimpleBankingAPI.DTOs.Request;
using SimpleBankingAPI.DTOs.Response;
using SimpleBankingAPI.Model;
using SimpleBankingAPI.Repository.Interface;

namespace SimpleBankingAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IBankingService _bankingService;

    public AccountsController(IBankingService bankingService)
    {
        _bankingService = bankingService;
    }
    
    [HttpPost("OpenAccount")]

    public async Task<ActionResult<ApiResponse<AccountResponse>>> OpenAccountAsync(CreateAccountRequest request)
    {
        var result = await _bankingService.CreateAccountAsync(request);
        if(!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("{accountNumber}")]
    public async Task<ActionResult<ApiResponse<AccountResponse>>> GetAccountAsync(string accountNumber)
    {
        var result = await _bankingService.GetAccountAsync(accountNumber);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<AccountResponse>>>> GetAllAccountsAsync()
    {
        var result = await _bankingService.GetAllAccountsAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPut("{accountNumber}")]
    public async Task<ActionResult<ApiResponse<AccountResponse>>> UpdateAccountAsync(string accountNumber, UpdateAccountRequest request)
    {
        var result = await _bankingService.UpdateAccountAsync(accountNumber, request);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpDelete("{accountNumber}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteAccountAsync(string accountNumber)
    {
        var result = await _bankingService.DeleteAccountAsync(accountNumber);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("{accountNumber}/Balance")]
    public async Task<ActionResult<ApiResponse<BalanceResponse>>> CheckBalanceAsync(string accountNumber)
    {
        var result = await _bankingService.CheckBalanceAsync(accountNumber);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        return Ok(result);
    }
    
    [HttpPost("Deposit")]

    public async Task<ActionResult<ApiResponse<decimal>>> Deposit(DepositRequest request)
    {
        var result = await _bankingService.DepositAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("Withdraw")]
    public async Task<ActionResult<ApiResponse<decimal>>> Withdraw(WithdrawRequest request)
    {
        var result = await _bankingService.WithdrawAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("FundTransfer")]
    public async Task<ActionResult<ApiResponse<TransferSummary>>> Transfer(TransferRequest request)
    {
        var result = await _bankingService.TransferFundsAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    
    
}