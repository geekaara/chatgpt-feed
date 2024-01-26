using Microsoft.EntityFrameworkCore;

public class BankingContext : DbContext
{
    public BankingContext()
    {
    }

    public BankingContext(DbContextOptions<BankingContext> options) : base(options)
    {
    }

public DbSet<Customer> Customers { get; set; }
public DbSet<Account> Accounts { get; set; }
public DbSet<Login> Logins { get; set; }
public DbSet<BillPay> BillPays { get; set; }
public DbSet<Payee> Payees { get; set; }
public DbSet<Transaction> Transactions { get; set; }

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer("server=rmit.australiaeast.cloudapp.azure.com;Encrypt=False;uid=s3956627_a2;database=s3956627_a2;pwd=abc123");
}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // I set the primary keys here
    modelBuilder.Entity<Customer>().HasKey(c => c.CustomerID);
    modelBuilder.Entity<Account>().HasKey(a => a.AccountNumber);
    modelBuilder.Entity<Login>().HasKey(l => l.LoginID);
    modelBuilder.Entity<BillPay>().HasKey(b => b.BillPayID);
    modelBuilder.Entity<Payee>().HasKey(p => p.PayeeID);
    modelBuilder.Entity<Transaction>().HasKey(t => t.TransactionID);

    modelBuilder.Entity<Customer>()
        .HasOne(c => c.Login)
        .WithOne(l => l.Customer)
        .HasForeignKey<Login>(l => l.CustomerID);

    modelBuilder.Entity<Account>()
        .HasMany(a => a.BillPays)
        .WithOne(b => b.Account)
        .HasForeignKey(b => b.AccountNumber);

    modelBuilder.Entity<Account>()
        .HasMany(a => a.Transactions)
        .WithOne(t => t.Account)
        .HasForeignKey(t => t.AccountNumber);

    modelBuilder.Entity<Payee>()
        .HasMany(p => p.BillPays)
        .WithOne(b => b.Payee)
        .HasForeignKey(b => b.PayeeID);
}
}