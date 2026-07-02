namespace SimpleBankingAPI.Repository.Interface;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail, string customerName, string accountNumber, decimal balance);
    Task SendAccountUpdateEmailAsync(string toEmail, string customerName, string accountNumber);
    Task SendAccountClosureEmailAsync(string toEmail, string customerName, string accountNumber);
    Task SendDepositEmailAsync(string toEmail,string customerName,string accountNumber,decimal newBalance,decimal depositAmount, string reference);
    Task SendWithdrawalEmailAsync(string toEmail,string customerName,string accountNumber,decimal newBalance,decimal withdrawalAmount, string reference);
    Task SendTransferDebitEmailAsync(string toEmail,string customerName,string senderAccountNumber,string receiverAccountNumber,decimal newBalance,decimal transferAmount, string reference);
    Task SendTransferCreditEmailAsync(string toEmail,string customerName,string senderAccountNumber,string receiverAccountNumber,decimal newBalance,decimal transferAmount, string reference);
    Task SendCheckBalanceEmailAsync(string toEmail, string customerName, string accountNumber, decimal balance);


}

