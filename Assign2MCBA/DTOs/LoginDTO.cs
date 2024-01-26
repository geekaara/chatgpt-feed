namespace Assign2MCBA.DTOs;

public class LoginDTO
{
    public string? LoginID { get; set; }
    public int CustomerID { get; set; }
    public string? PasswordHash { get; set; }
    public CustomerDTO? Customer { get; set; }
}