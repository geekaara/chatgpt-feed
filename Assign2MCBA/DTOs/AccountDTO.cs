namespace Assign2MCBA.DTOs;

public class AccountDTO
{
    public int AccountNumber { get; set; }
    public string? AccountType { get; set; }
    public int CustomerID { get; set; }
    public virtual IEnumerable<TransactionDTO>? Transactions { get; set; }
    public IEnumerable<BillPayDTO>? BillPays { get; set; }
    public virtual CustomerDTO? Customer { get; set; }
}