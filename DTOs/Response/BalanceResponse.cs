namespace SimpleBankingAPI.DTOs.Response;

public class BalanceResponse
{
    public string AccountNumber { get; set; }
    
    public string AccountName { get; set; }
    
    public decimal Balance { get; set; }
}