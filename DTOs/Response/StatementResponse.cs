using SimpleBankingAPI.Model;

namespace SimpleBankingAPI.DTOs.Response;

public class StatementResponse
{
    public string AccountNumber { get; set; }
    
    public string CustomerName { get; set; }
    
    public DateTime FromDate { get; set; }
    
    public DateTime ToDate { get; set; }
    
    public decimal OpeningBalance { get; set; }
    
    public decimal ClosingBalance { get; set; }
    
    public decimal TotalCredit { get; set; }
    
    public decimal TotalDebit { get; set; }
    
    public int TransactionCount {get; set;}
    
    public IEnumerable<TransactionResponse> Transactions { get; set; }
}