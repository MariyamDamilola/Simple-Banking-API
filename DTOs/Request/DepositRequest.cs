namespace SimpleBankingAPI.Model;

public class DepositRequest
{
    public string AccountNumber { get; set; }
    
    public string Narration { get; set; }
    
    public decimal Amount { get; set; }
}