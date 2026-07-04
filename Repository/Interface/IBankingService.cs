using SimpleBankingAPI.DTOs.Request;
using SimpleBankingAPI.DTOs.Response;
using SimpleBankingAPI.Model;

namespace SimpleBankingAPI.Repository.Interface;

public interface IBankingService
{
    //CRUD operation for Account
    Task<ApiResponse<AccountResponse>> CreateAccountAsync(CreateAccountRequest request);
    
    Task<ApiResponse<AccountResponse>> GetAccountAsync(string accountNumber);
    
    Task<ApiResponse<AccountResponse>> UpdateAccountAsync(string accountNumber, UpdateAccountRequest request);
    
    Task<ApiResponse<bool>> DeleteAccountAsync(string accountNumber);
    
    Task<ApiResponse<bool>> RestoreAccountAsync(string accountNumber);

    
    Task<ApiResponse<IEnumerable<AccountResponse>>> GetAllAccountsAsync();
    
    // Bank Operations for Account 
    Task<ApiResponse<TransferSummary>> TransferFundsAsync(TransferRequest request);
    
    Task<ApiResponse<decimal>> DepositAsync(DepositRequest request);
    
    Task<ApiResponse<decimal>> WithdrawAsync(WithdrawRequest request);
    
    Task<ApiResponse<BalanceResponse>> CheckBalanceAsync(string accountNumber);
    
    Task<ApiResponse<StatementResponse>> GetTransactionsHistoryAsync(string accountNumber, DateTime? fromDate = null, DateTime? toDate = null);
    
    ApiResponse<BankInfoResponse> GetBankInfo();

}