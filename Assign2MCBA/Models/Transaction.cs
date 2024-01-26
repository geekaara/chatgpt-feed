public class Transaction
{
    public int TransactionID { get; set; }
    public string? TransactionType { get; set; }
    public int AccountNumber { get; set; }
    public int? DestinationAccountNumber { get; set; } // Nullable for transactions that don't involve a transfer
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public DateTime TransactionTimeUtc { get; set; }
public virtual Account? Account { get; set; }
}