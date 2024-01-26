namespace Assign2MCBA.DTOs;

public class TransactionDTO
{
    public int TransactionID { get; set; }
    public string? TransactionType { get; set; }
    public int AccountNumber { get; set; }
    public int? DestinationAccountNumber { get; set; } // Nullable for transactions that don't involve a transfer
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public string TransactionTimeUtc { get; set; }
}