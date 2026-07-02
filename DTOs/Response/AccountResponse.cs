namespace SimpleBankingAPI.DTOs.Response;

public class AccountResponse
{
    public string AccountNumber { get; set; }
    
    public string AccountName { get; set; }
    
    public decimal Balance { get; set; }
    
    public string Email { get; set; }
    
    public DateTime CreatedAt { get; set; }
}