using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleBankingAPI.DTOs.Request;
using SimpleBankingAPI.DTOs.Response;
using SimpleBankingAPI.Model;
using SimpleBankingAPI.Repository.Interface;

namespace SimpleBankingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IBankingService _bankingService;
    private readonly BankInfo _bankInfo;

    public AccountController(IBankingService bankingService, IOptions<BankInfo> bankInfoOptions)
    {
        _bankingService = bankingService;
        _bankInfo = bankInfoOptions.Value;
    }


    [HttpPost("Open-Account")]

    public async Task<ActionResult<ApiResponse<AccountResponse>>> OpenAccountAsync(CreateAccountRequest request)
    {
        var result = await _bankingService.CreateAccountAsync(request);
        if (!result.IsSuccess)
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


    [HttpPut("{accountNumber}")]
    public async Task<ActionResult<ApiResponse<AccountResponse>>> UpdateAccountAsync(string accountNumber,
        UpdateAccountRequest request)
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


    [HttpPost("Activate-account/{accountNumber}")]
    public async Task<ActionResult<ApiResponse<bool>>> RestoreAccountAsync(string accountNumber)
    {
        var result = await _bankingService.RestoreAccountAsync(accountNumber);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("All-Accounts")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AccountResponse>>>> GetAllAccountsAsync()
    {
        var result = await _bankingService.GetAllAccountsAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }



    [HttpPost("deposit")]

    public async Task<ActionResult<ApiResponse<decimal>>> Deposit(DepositRequest request)
    {
        var result = await _bankingService.DepositAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("withdraw")]
    public async Task<ActionResult<ApiResponse<decimal>>> Withdraw(WithdrawRequest request)
    {
        var result = await _bankingService.WithdrawAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("transfer-fund")]
    public async Task<ActionResult<ApiResponse<TransferSummary>>> Transfer(TransferRequest request)
    {
        var result = await _bankingService.TransferFundsAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("balance/{accountNumber}")]
    public async Task<ActionResult<ApiResponse<BalanceResponse>>> CheckBalanceAsync(string accountNumber)
    {
        var result = await _bankingService.CheckBalanceAsync(accountNumber);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpGet("bank-name")]
    public ActionResult GetBankInfo()
    {
        return Ok(new
        {
            isSuccess = true,
            message = "Bank info retrieved successfully",
            data = new
            {
                bankName = _bankInfo.BankName,
                description = _bankInfo.Description
            }

        });
    }
    
    [HttpGet("statement/{accountNumber}")]
    public async Task<ActionResult<ApiResponse<StatementResponse>>> GetStatementAsync(string accountNumber, DateTime? fromDate, DateTime? toDate)
    {
        var result = await _bankingService.GetTransactionsHistoryAsync(accountNumber, fromDate, toDate);
        if (!result.IsSuccess) BadRequest(result);
        return Ok(result);
    }
}

            
            
    