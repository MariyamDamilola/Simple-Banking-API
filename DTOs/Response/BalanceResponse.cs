namespace SimpleBankingAPI.DTOs.Response;

public class BalanceResponse
{
    public string AccountNumber { get; set; }
    
    public string CustomerName { get; set; }
    
    public decimal Balance { get; set; }
}