namespace SimpleBankingAPI.DTOs.Request;

public class TransferRequest
{
    public string SenderAccountNumber { get; set; }
    
    public string ReceiverAccountNumber { get; set; }
    
    public decimal Amount { get; set; }
    
    public string Narration { get; set; }
}