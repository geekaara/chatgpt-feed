namespace Assign2MCBA.DTOs;

public class CustomerDTO
{
    public int CustomerID { get; set; }
    public string? Name { get; set; }
    public string? TFN { get; set; } 
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Postcode { get; set; }
    public string? Mobile { get; set; }
    public IEnumerable<AccountDTO>? Accounts { get; set; }
    public LoginDTO? Login { get; set; }
}