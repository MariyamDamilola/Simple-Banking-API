namespace SimpleBankingAPI.DTOs.Response;

public class TransferSummary
{
    public string TransactionReference { get; set; }
    
    public string SenderAccount { get; set; }
    
    public string ReceiverAccount { get; set; }
    
    public decimal Amount { get; set; }
    
    public decimal SenderNewBalance { get; set; }
    
    public DateTime CreatedAt { get; set; }
}