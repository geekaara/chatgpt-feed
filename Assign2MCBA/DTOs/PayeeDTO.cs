namespace Assign2MCBA.DTOs;

public class PayeeDTO
{
    public int PayeeID { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Postcode { get; set; }
    public string? Phone { get; set; }
    public IEnumerable<BillPayDTO>? BillPays { get; set; }
}