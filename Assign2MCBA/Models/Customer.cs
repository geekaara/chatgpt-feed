using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class Customer
{
    public int CustomerID { get; set; }
    public string? Name { get; set; }
    public string? TFN { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Postcode { get; set; }
    public string? Mobile { get; set; }
    public virtual ICollection<Account>? Accounts { get; set; }
    public virtual Login? Login { get; set; }
}