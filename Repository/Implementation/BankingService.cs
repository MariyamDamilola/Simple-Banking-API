using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleBankingAPI.Data;
using SimpleBankingAPI.DTOs.Request;
using SimpleBankingAPI.DTOs.Response;
using SimpleBankingAPI.Model;
using SimpleBankingAPI.Repository.Interface;
using SimpleBankingAPI.Utilities;

namespace SimpleBankingAPI.Repository.Implementation;

public class BankingService : IBankingService
{
    private readonly BankingDbContext _context;
    private readonly ILogger<BankingService> _logger;
    private readonly IEmailService _emailService;
    private readonly BankInfo _bankInfo;


    public BankingService(BankingDbContext context, ILogger<BankingService> logger, IEmailService emailService, IOptions<BankInfo> bankInfo)
    {
        _context = context;
        _logger = logger;
        _emailService = emailService;
        _bankInfo = bankInfo.Value;
    }
    
    
    public async Task<ApiResponse<AccountResponse>> CreateAccountAsync(CreateAccountRequest request)
    {
        try
        {
            var checkEmail = await _context.Accounts.AnyAsync(x => x.Email == request.Email);
            if (checkEmail)
            {
                _logger.LogWarning($"Account creation rejected: Email already exists for {request.Email}");
                return ApiResponse<AccountResponse>.FailureResponse("Email already exists");
            }

            var newAcct = new Account()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                AccountNumber = AccountNumberGenerator.GenerateAcc(),
                Balance = 0.00m,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Accounts.Add(newAcct);
            await _context.SaveChangesAsync();
            
           _logger.LogInformation($"Successfully created account new account {newAcct.AccountNumber} for customer {request.FirstName} {request.LastName}");
           
         await  _emailService.SendWelcomeEmailAsync(newAcct.Email, $"{newAcct.FirstName} {newAcct.LastName}", newAcct.AccountNumber, newAcct.Balance);
           
           var response = new AccountResponse()
           {
               AccountNumber = newAcct.AccountNumber,
               CustomerName = $"{newAcct.FirstName} {newAcct.LastName}",
               Balance = newAcct.Balance,
               Email = newAcct.Email,
               CreatedAt = newAcct.CreatedAt
           };
           
           return ApiResponse<AccountResponse>.Success(response);
           

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occured during account creation for customer {request.FirstName} {request.LastName}");
            return ApiResponse<AccountResponse>.FailureResponse("Error occured during account creation");
        }
    }

    public async Task<ApiResponse<AccountResponse>> GetAccountAsync(string accountNumber)
    {
        try
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber && !x.IsDeleted);

            if (account == null)
            {
                _logger.LogWarning($"Account look-up failed: Account number {accountNumber} does not exist or is inactive.");
                return ApiResponse<AccountResponse>.FailureResponse("Account not found.");
            }

            var response = new AccountResponse()
            {
                AccountNumber = account.AccountNumber,
                CustomerName = $"{account.FirstName} {account.LastName}",
                Balance = account.Balance,
                Email = account.Email,
                CreatedAt = account.CreatedAt
            };
            return ApiResponse<AccountResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while fetching account number {accountNumber}");
            return ApiResponse<AccountResponse>.FailureResponse("An error occurred while retrieving the account.");
        }
    }

    public async Task<ApiResponse<AccountResponse>> UpdateAccountAsync(string accountNumber, UpdateAccountRequest request)
    {
       try
       { var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber && !x.IsDeleted);
           if (account == null)
           {
               _logger.LogWarning($"Update failed: Account {accountNumber} not found or is inactive.");
               return ApiResponse<AccountResponse>.FailureResponse("Account not found.");
           }
           
           account.FirstName = request.FirstName;
           account.LastName = request.LastName;
       
           _context.Accounts.Update(account);
           await _context.SaveChangesAsync();
       
           _logger.LogInformation($"Successfully updated profile details for account {accountNumber}.");
           await _emailService.SendAccountUpdateEmailAsync(account.Email, $"{account.FirstName} {account.LastName}", account.AccountNumber);
           
           var response = new AccountResponse()
           {
               AccountNumber = account.AccountNumber,
               CustomerName = $"{account.FirstName} {account.LastName}",
               Balance = account.Balance,
               Email = account.Email,
               CreatedAt = account.CreatedAt
           };
       
           return ApiResponse<AccountResponse>.Success(response);
       }
       catch (Exception ex)
       {
           _logger.LogError(ex, $"Error occurred while updating account profile for {accountNumber}.");
           return ApiResponse<AccountResponse>.FailureResponse("An error occurred while updating the account.");
       }      
    }

    public async Task<ApiResponse<bool>> DeleteAccountAsync(string accountNumber)
    {
        try
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber && !x.IsDeleted);
            if (account == null)
            {
                _logger.LogWarning($"Delete failed: Active account {accountNumber} not found.");
                return ApiResponse<bool>.FailureResponse("Account not found or already deactivated.");
            }
            
            if (account.Balance > 0)
            {
                _logger.LogWarning($"Delete rejected: Account {accountNumber} still has a remaining balance of {account.Balance}.");
                return ApiResponse<bool>.FailureResponse("Cannot delete an account with an active balance.");
            }
            
            account.IsDeleted = true;
        
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully soft-deleted account {accountNumber}.");
            await _emailService.SendAccountClosureEmailAsync(account.Email, $"{account.FirstName} {account.LastName}", account.AccountNumber);
            
            return ApiResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while soft-deleting account {accountNumber}.");
            return ApiResponse<bool>.FailureResponse("An error occurred while deleting the account.");
        }
    }
    
    public async Task<ApiResponse<bool>> RestoreAccountAsync(string accountNumber)
    {
        try
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber && x.IsDeleted);
            if (account == null)
            {
                _logger.LogWarning($"Restore failed: Deleted account {accountNumber} not found.");
                return ApiResponse<bool>.FailureResponse("Account not found or is not currently deleted.");
            }
    
            account.IsDeleted = false;
    
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
    
            _logger.LogInformation($"Successfully restored account {accountNumber}.");
            await _emailService.SendAccountRestoredEmailAsync(account.Email, $"{account.FirstName} {account.LastName}", account.AccountNumber);
            
            return ApiResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while restoring account {accountNumber}.");
            return ApiResponse<bool>.FailureResponse("An error occurred while restoring the account.");
        }
    }

    public async Task<ApiResponse<IEnumerable<AccountResponse>>> GetAllAccountsAsync()
    {
        try
        {
            var accounts = await _context.Accounts
                .Where(account => !account.IsDeleted)
                .Select(account => new AccountResponse
                {
                    AccountNumber = account.AccountNumber,
                    CustomerName = $"{account.FirstName} {account.LastName}",
                    Balance = account.Balance,
                    Email = account.Email,
                    CreatedAt = account.CreatedAt
                })
                .ToListAsync();

            return ApiResponse<IEnumerable<AccountResponse>>.Success(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all accounts.");
            return ApiResponse<IEnumerable<AccountResponse>>.FailureResponse(
                "An error occurred while retrieving accounts.");
        }
    }

    public async Task<ApiResponse<TransferSummary>> TransferFundsAsync(TransferRequest request)
    {
        try
        {
            if (request.SenderAccountNumber == request.ReceiverAccountNumber)
            {
                _logger.LogWarning($"Transfer rejected: Sender and receiver account cannot be the same ({request.SenderAccountNumber}).");
                return ApiResponse<TransferSummary>.FailureResponse("Sender and receiver account cannot be the same");
            }

            if (request.Amount <= 0)
            {
                _logger.LogError($"Transfer failed: Invalid amount {request.Amount} requested from {request.SenderAccountNumber} to {request.ReceiverAccountNumber}");
                return ApiResponse<TransferSummary>.FailureResponse("Amount must be greater than zero");
            }
            
            var senderAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == request.SenderAccountNumber && !x.IsDeleted);
            var receiverAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == request.ReceiverAccountNumber && !x.IsDeleted);
            
            if (senderAccount == null || receiverAccount == null)
            {
                _logger.LogWarning($"Transfer failed: Sender or receiver account not found or inactive");
                return ApiResponse<TransferSummary>.FailureResponse("Sender or receiver account not found or inactive");
            }

            if (senderAccount.Balance < request.Amount)
            {
                _logger.LogWarning($"Transfer rejected: Insufficient balance for sender account {request.SenderAccountNumber}");
                return ApiResponse<TransferSummary>.FailureResponse("Insufficient balance for sender account");
            }
            
            senderAccount.Balance -= request.Amount; 
            receiverAccount.Balance += request.Amount; 

            var reference = TransactionRefGenerator.GenerateRef("TRF");

            var debitTransaction = new Transaction()
            {
                AccountNumber = senderAccount.AccountNumber,
                Amount = request.Amount,
                Reference = $"D-{reference}",
                Description = $"Debit of {request.Amount} from account {senderAccount.AccountNumber}",
                TransactionType = "Debit",
                Narration = request.Narration,
                CreatedAt = DateTime.UtcNow
            };
            

            var creditTransaction = new Transaction()
            {
                AccountNumber = receiverAccount.AccountNumber,
                Amount = request.Amount,
                Reference = $"C-{reference}",
                Description = $"Credit of {request.Amount} to account {receiverAccount.AccountNumber}",
                TransactionType = "Credit",
                Narration = request.Narration,
                CreatedAt = DateTime.UtcNow
            };
         
            _context.Transactions.AddRange(debitTransaction, creditTransaction);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Successfully transferred {request.Amount} from account {senderAccount.AccountNumber} to account {receiverAccount.AccountNumber}");
            
            
          await  _emailService.SendTransferDebitEmailAsync(senderAccount.Email,
                $"{senderAccount.FirstName} {senderAccount.LastName}",
                senderAccount.AccountNumber, receiverAccount.AccountNumber,
                senderAccount.Balance, request.Amount, debitTransaction.Reference);
            
         
          await _emailService.SendTransferCreditEmailAsync(receiverAccount.Email,
              $" {receiverAccount.FirstName} {receiverAccount.LastName}",
              senderAccount.AccountNumber, receiverAccount.AccountNumber,
              receiverAccount.Balance, request.Amount,creditTransaction.Reference);
          
           
            var summary = new TransferSummary()
            {
                TransactionReference = debitTransaction.Reference,
                SenderAccount = senderAccount.AccountNumber,
                ReceiverAccount = receiverAccount.AccountNumber,
                Amount = request.Amount,
                SenderNewBalance = senderAccount.Balance,
                CreatedAt = DateTime.UtcNow
            };
            return ApiResponse<TransferSummary>.Success(summary, "Fund transfer processed successfully");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred during transfer processing from {request.SenderAccountNumber} to {request.ReceiverAccountNumber}");
            return ApiResponse<TransferSummary>.FailureResponse("An unexpected processing error occurred during the transfer.");
        
        }
    }

    public async Task<ApiResponse<decimal>> DepositAsync(DepositRequest request)
    {
        try
        {
            if (request.Amount <= 0)
            {
                _logger.LogError($"Deposit failed: Invalid amount {request.Amount} requested for account {request.AccountNumber}");
                return ApiResponse<decimal>.FailureResponse("Amount must be greater than zero");
            }
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == request.AccountNumber && !x.IsDeleted);
            if (account == null)
            {
                _logger.LogWarning($"Deposit failed: Account not found for account number {request.AccountNumber}");
                return ApiResponse<decimal>.FailureResponse("Account not found");
            }
            
            account.Balance += request.Amount; 
            
            var reference = TransactionRefGenerator.GenerateRef("DEP");
            var transaction = new Transaction()
            {
                AccountNumber = account.AccountNumber,
                Amount = request.Amount,
                Reference = reference,
                Description = $"Deposit of {request.Amount} to account {account.AccountNumber}",
                TransactionType = "Deposit",
                Narration = request.Narration,
                CreatedAt = DateTime.UtcNow
            };
            _context.Transactions.Add(transaction); 
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Successfully deposited {request.Amount} to account {account.AccountNumber}");
            
         
        await _emailService.SendDepositEmailAsync(account.Email, $"{account.FirstName} {account.LastName}", account.AccountNumber, account.Balance, request.Amount, reference);
            
            return ApiResponse<decimal>.Success(account.Balance,"Deposit processed successfully");
        }
        catch (Exception ex)
        {
           _logger.LogError(ex, $"Error occured during deposit for account {request.AccountNumber}");
            return ApiResponse<decimal>.FailureResponse($"Error occured during deposit");
        }
    }

    public async Task<ApiResponse<decimal>> WithdrawAsync(WithdrawRequest request)
    {
        try
        {
            if (request.Amount <= 0)
            {
                _logger.LogError($"Withdrawal failed: Invalid amount {request.Amount} requested for account {request.AccountNumber}");
                return ApiResponse<decimal>.FailureResponse("Amount must be greater than zero");
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == request.AccountNumber && !x.IsDeleted);
            if (account == null)
            {
                _logger.LogWarning($"Withdrawal failed: Account not found for account number {request.AccountNumber}");
                return ApiResponse<decimal>.FailureResponse($"Account number {request.AccountNumber} does not exist");
            }
            
           
            if (account.Balance < request.Amount)
            {
                _logger.LogWarning($"Withdrawal rejected: Insufficient balance for account number {request.AccountNumber}");
                return ApiResponse<decimal>.FailureResponse($"Insufficient balance for account number {request.AccountNumber}");
            }
            
            account.Balance -= request.Amount; 
            
            var reference = TransactionRefGenerator.GenerateRef("WDR");
            var transaction = new Transaction()
            {
                AccountNumber = account.AccountNumber,
                TransactionType = "Withdrawal",
                Reference = reference,
                Narration = "",
                Amount = request.Amount,
                 Description = $"Withdrawal of {request.Amount} from account {account.AccountNumber}",
                CreatedAt = DateTime.UtcNow
            };
            _context.Transactions.Add(transaction); 
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Successfully withdrawn {request.Amount} from account {account.AccountNumber}");
            
            await _emailService.SendWithdrawalEmailAsync(account.Email, $"{account.FirstName} {account.LastName}", account.AccountNumber, account.Balance, request.Amount, reference);
            
            return ApiResponse<decimal>.Success(account.Balance,"Withdrawal processed successfully");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occured during withdrawal for account {request.AccountNumber}");
            return ApiResponse<decimal>.FailureResponse($"Error occured during withdrawal");
        }
    }

    public async Task<ApiResponse<BalanceResponse>> CheckBalanceAsync(string accountNumber)
    {
        try
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber && !x.IsDeleted);
            
            if (account == null)
            {
                _logger.LogWarning($"Balance check failed: Active account {accountNumber} not found.");
                return ApiResponse<BalanceResponse>.FailureResponse("Account not found or is inactive.");
            }
            
            var response = new BalanceResponse()
            {
                AccountNumber = account.AccountNumber,
                CustomerName = $"{account.FirstName} {account.LastName}",
                Balance = account.Balance
            };
            
            await _emailService.SendCheckBalanceEmailAsync(account.Email, $"{account.FirstName} {account.LastName}", account.AccountNumber, account.Balance);

            return ApiResponse<BalanceResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while checking balance for account {accountNumber}.");
            return ApiResponse<BalanceResponse>.FailureResponse("An error occurred while retrieving the balance.");
        }
    }

    
      private static readonly HashSet<string> GetCreditTransactionTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "Deposit", "Credit"
        };

      
     public async Task<ApiResponse<StatementResponse>> GetTransactionsHistoryAsync(string accountNumber, DateTime? fromDate, DateTime? toDate)
{
    try
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

        if (account == null)
        {
            _logger.LogError($"Account with account number {accountNumber} not found");
            return ApiResponse<StatementResponse>.FailureResponse("Account not found");
        }

        if (account.IsDeleted)
        {
            _logger.LogError($"Attempted access to deactivated account: {account.AccountNumber} - GetTransactionsHistory");
            return ApiResponse<StatementResponse>.FailureResponse("This account has been deactivated.");
        }

        if (fromDate.HasValue && toDate.HasValue && fromDate.Value.Date > toDate.Value.Date)
        {
            _logger.LogError($"From date is later than to date - transaction history date");
            return ApiResponse<StatementResponse>.FailureResponse("FromDate cannot be later than ToDate");
        }

        var startDate = fromDate?.Date;
        var endDate = toDate?.Date;

        // 1. CHANGED: Filter by AccountNumber and fetch to memory first to avoid EF sorting translation limits
        var dbTransactions = await _context.Transactions
            .Where(x => x.AccountNumber == account.AccountNumber)
            .ToListAsync();

        // 2. FIXED: Changed 't.Date' to 't.CreatedAt' to match your Transaction model
        var allTransactions = dbTransactions.OrderBy(t => t.CreatedAt).ToList();

        decimal TransactionAmount(Transaction transaction)
        {
            return GetCreditTransactionTypes.Contains(transaction.TransactionType)
                ? transaction.Amount : -transaction.Amount;
        }

        // FIXED: Changed 'x.Date' to 'x.CreatedAt'
        var openingBalance = startDate.HasValue ? allTransactions.Where(x => x.CreatedAt < startDate.Value).Sum(TransactionAmount) : 0;
        
        // trx counts within range
        // FIXED: Changed 't.Date' to 't.CreatedAt'
        var transactionsInRange = allTransactions
            .Where(t => (!startDate.HasValue || t.CreatedAt >= startDate.Value) && (!endDate.HasValue || t.CreatedAt <= endDate.Value))
            .ToList();
        
        //get all credits transactions 
        var totalCredits = transactionsInRange.Where(t => GetCreditTransactionTypes.Contains(t.TransactionType)).Sum(t => t.Amount);
        // get all debits trxs
        var totalDebits = transactionsInRange.Where(t => !GetCreditTransactionTypes.Contains(t.TransactionType)).Sum(t => t.Amount);
        
        // get closing balance
        var closingBalance = openingBalance + totalCredits - totalDebits;

        var response = new StatementResponse()
        {
            AccountNumber = account.AccountNumber,
            CustomerName = $"{account.FirstName} {account.LastName}",
           FromDate = fromDate ?? DateTime.MinValue, 
               ToDate = toDate ?? DateTime.UtcNow,
            OpeningBalance = openingBalance,
            ClosingBalance = closingBalance,
            TotalCredit = totalCredits,
            TotalDebit = totalDebits,
            TransactionCount = transactionsInRange.Count, // Changed from Count() to Count
            
            // FIXED: Changed 't.Date' to 't.CreatedAt', 't.Naration' to 't.Narration', and added .ToList()
            Transactions = transactionsInRange.OrderByDescending(t => t.CreatedAt)
                .Select(t => new TransactionResponse
                {
                    Type = t.TransactionType,
                    AccountNumber = t.AccountNumber,
                    Amount = t.Amount,
                    Narration = t.Narration, 
                    Reference = t.Reference,
                    CreatedAt = t.CreatedAt,
                }).ToList()
        };
        
        _logger.LogInformation($"Successfully retrieved {response.TransactionCount} transactions for account {account.AccountNumber}");
        
        return ApiResponse<StatementResponse>.Success(response);
    }
    catch (Exception e)
    {
       // FIXED: Passed exception 'e' as the first parameter to log the actual stack trace
       _logger.LogError(e, "Failed to get transactions history");
       return ApiResponse<StatementResponse>.FailureResponse("Failed to get transactions history");
    }
}

    public ApiResponse<BankInfoResponse> GetBankInfo()
    {
        var response = new BankInfoResponse
        {
            BankName = _bankInfo.BankName,
            Description = _bankInfo.Description
        };

        return ApiResponse<BankInfoResponse>.Success(response);
    }
}