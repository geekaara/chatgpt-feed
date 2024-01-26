using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Assign2MCBA.Models;
using Microsoft.Data.SqlClient;
using SimpleHashing.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Assign2MCBA.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BankingContext _dbContext;

    public HomeController(ILogger<HomeController> logger, BankingContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(Login loginModel)
    {
        if (!int.TryParse(loginModel.LoginID, out int loginIdNumeric))
        {
            // Invalid login ID format (contains non-numeric characters)
            ModelState.AddModelError(string.Empty, "Invalid login ID format");
            return View(loginModel);
        }
        var user = _dbContext.Logins.FirstOrDefaultAsync(l => l.LoginID == loginModel.LoginID );
        if (user != null)
        {
                // Successful login, redirect to the Index action
                return RedirectToAction("Index");
        }

        // Invalid login or password, display an error message
        ModelState.AddModelError(string.Empty, "Invalid login credentials");
        return View(loginModel);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Deposit()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(Transaction transaction)
    {
        if(ModelState.IsValid)
        {
            var Account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.AccountNumber == transaction.AccountNumber);

            if(Account != null)
            {
                Transaction transaction1 = new Transaction
                {
                    TransactionType = "D",
                    AccountNumber = transaction.AccountNumber,
                    Amount = transaction.Amount,
                    Comment = transaction.Comment,
                    TransactionTimeUtc = DateTime.UtcNow

                };

                _dbContext.Add(transaction1);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No Account found");
            }
        }
        return View("Index", transaction);
    }
    public IActionResult Withdraw()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Withdraw(Transaction transaction)
    {
        if(ModelState.IsValid)
        {
            var Account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.AccountNumber == transaction.AccountNumber);

            if(Account != null)
            {
                var withdrawCount = Convert.ToInt32(HttpContext.Session.GetString("WithdrawCount") ?? "0");
                withdrawCount++;
                HttpContext.Session.SetString("WithdrawCount", withdrawCount.ToString());

                if (withdrawCount > 2)
                {
                    Transaction transaction1 = new Transaction
                    {
                        TransactionType = "W",
                        AccountNumber = transaction.AccountNumber,
                        Amount = transaction.Amount,
                        Comment = transaction.Comment,
                        TransactionTimeUtc = DateTime.UtcNow

                    };
                    Transaction transaction2 = new Transaction
                    {
                        TransactionType = "S",
                        AccountNumber = transaction.AccountNumber,
                        Amount = 0.05m,
                        Comment = transaction.Comment,
                        TransactionTimeUtc = DateTime.UtcNow

                    };
                    _dbContext.Add(transaction1);
                    _dbContext.Add(transaction2);
                }
                else
                {
                    Transaction transaction3 = new Transaction
                    {
                        TransactionType = "W",
                        AccountNumber = transaction.AccountNumber,
                        Amount = transaction.Amount,
                        Comment = transaction.Comment,
                        TransactionTimeUtc = DateTime.UtcNow
                    };
                    _dbContext.Add(transaction3);
                }
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No Account found");
            }
        }
        return View("Index", transaction);
    }
    public IActionResult Transfer()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Transfer(Transaction sourceTransaction, int destinationAccountNumber)
    {
        if (!ModelState.IsValid)
        {
            return View(sourceTransaction);
        }

        try
        {
            var sourceAccount = await _dbContext.Accounts
                .FirstOrDefaultAsync(acc => acc.AccountNumber == sourceTransaction.AccountNumber);
            var destinationAccount = await _dbContext.Accounts
                .FirstOrDefaultAsync(acc => acc.AccountNumber == destinationAccountNumber);

            if (sourceAccount == null || destinationAccount == null)
            {
                ModelState.AddModelError(string.Empty, "One or both account numbers are invalid.");
                return View(sourceTransaction);
            }

            // Create transaction records for the withdrawal and deposit
            var withdrawalTransaction = new Transaction
            {
                TransactionType = "T",
                AccountNumber = sourceTransaction.AccountNumber,
                Amount = sourceTransaction.Amount,
                Comment = sourceTransaction.Comment,
                DestinationAccountNumber = destinationAccountNumber,
                TransactionTimeUtc = DateTime.UtcNow,
            };

            _dbContext.Transactions.Add(withdrawalTransaction);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            // Log the detailed exception
            _logger.LogError(ex, "Error during fund transfer");
            ModelState.AddModelError(string.Empty, "An error occurred during the transaction. Please try again.");
            return View(sourceTransaction);
        }
    }


    public IActionResult MyStatements()
    {
        return View();
    }
    public IActionResult MyProfile()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
