namespace SimpleBankingAPI.Model;

public class Account
{
    
    public Guid Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string AccountNumber { get; set; }
    
    public string Email { get; set; }
    
    public decimal Balance { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime CreatedAt { get; set; }
}