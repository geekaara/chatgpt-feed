public class BillPay
{
    public int BillPayID { get; set; }
    public int AccountNumber { get; set; }
    public int PayeeID { get; set; }
    public decimal Amount { get; set; }
    public DateTime ScheduleTimeUtc { get; set; }
    public string? Period { get; set; }
    public virtual Account? Account { get; set; }
    public virtual Payee? Payee { get; set; }
}