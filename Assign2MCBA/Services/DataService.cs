using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Assign2MCBA.Models;
using Assign2MCBA.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Assign2MCBA.Services
{
    public class DataService
    {
        private readonly HttpClient _httpClient;
        private readonly BankingContext _context;

        public DataService(HttpClient httpClient, BankingContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task GetInitialDataAsync(string url)
        {
            if (await _context.Customers.AnyAsync())
                return;

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var customersDTO = JsonSerializer.Deserialize<List<CustomerDTO>>(content);

            foreach (var customerDTO in customersDTO)
            {
                var customer = new Customer
                {
                    CustomerID = customerDTO.CustomerID,
                    Name = customerDTO.Name,
                    Address = customerDTO.Address,
                    City = customerDTO.City,
                    Postcode = customerDTO.Postcode,
                    Accounts = new List<Account>()
                };

                foreach (var accountDTO in customerDTO.Accounts)
                {
                    var account = new Account
                    {
                        AccountNumber = accountDTO.AccountNumber,
                        AccountType = accountDTO.AccountType,
                        CustomerID = customer.CustomerID,
                        Transactions = new List<Transaction>()
                    };

                    foreach (var transactionDTO in accountDTO.Transactions)
                    {
                        if (DateTime.TryParseExact(
                            transactionDTO.TransactionTimeUtc,
                            "dd/MM/yyyy hh:mm:ss tt",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var transactionTime))
                        {
                            var transaction = new Transaction
                            {
                                TransactionType = "D", 
                                AccountNumber = account.AccountNumber,
                                Amount = transactionDTO.Amount,
                                Comment = transactionDTO.Comment,
                                TransactionTimeUtc = transactionTime
                            };

                            account.Transactions.Add(transaction);
                        }
                    }

                    customer.Accounts.Add(account);
                }

                _context.Customers.Add(customer);
            }

            await _context.SaveChangesAsync();
        }
    }
}

