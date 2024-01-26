namespace Assign2MCBA.DTOs;

public class BillPayDTO
{
    public int BillPayID { get; set; }
    public int AccountNumber { get; set; }
    public int PayeeID { get; set; }
    public decimal Amount { get; set; }
    public DateTime ScheduleTimeUtc { get; set; }
    public string? Period { get; set; }
    public AccountDTO? Account { get; set; }
    public PayeeDTO? Payee { get; set; }
}