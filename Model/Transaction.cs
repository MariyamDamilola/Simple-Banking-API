namespace SimpleBankingAPI.Model;

public class Transaction
{
    public Guid Id { get; set; }
    
    public string AccountNumber { get; set; }
   
    public string TransactionType { get; set; }
    
    public string Description { get; set; }
    public decimal Amount { get; set; }
    
    public string Narration { get; set; }
    
    public string Reference { get; set; }
    
    public DateTime CreatedAt { get; set; }
}