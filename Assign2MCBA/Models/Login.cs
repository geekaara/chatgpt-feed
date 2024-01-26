public class Login
{
    public string? LoginID { get; set; }
    public int CustomerID { get; set; }
    public string? PasswordHash { get; set; }
    public virtual Customer? Customer { get; set; }
}