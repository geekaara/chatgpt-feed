using Microsoft.Identity.Client;

public class Account
{
    public int AccountNumber { get; set; }
    public string? AccountType { get; set; }
    public int CustomerID { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<BillPay>? BillPays { get; set; }
    public virtual ICollection<Transaction>? Transactions { get; set; }
}