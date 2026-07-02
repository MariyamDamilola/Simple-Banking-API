namespace SimpleBankingAPI.DTOs.Response;

public class TransactionResponse
{
    public string Reference { get; set; }
    
    public string AccountNumber { get; set; }
    
    public string Narration { get; set; }
    
    public string Type { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
}