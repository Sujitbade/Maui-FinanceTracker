/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly string transactionsFilePath = Path.Combine(AppContext.BaseDirectory, "Transaction.json");

        public async Task AddTransactionAsync(Transaction transaction)
        {
            try
            {
                var transactions = await LoadTransactionsAsync();
                transaction.Id = transactions.Count > 0 ? transactions.Max(t => t.Id) + 1 : 1;
                transactions.Add(transaction);
                await SaveTransactionsAsync(transactions);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error adding transaction: {ex.Message}");
                throw; // Rethrow or handle as needed
            }
        }

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            try
            {
                var transactions = await LoadTransactionsAsync();
                // Return transactions filtered by userId or an empty list if transactions is null
                return (transactions ?? new List<Transaction>()).Where(t => t.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving transactions for user {userId}: {ex.Message}");
                return new List<Transaction>(); // Return an empty list in case of an exception
            }
        }

        private async Task<List<Transaction>> LoadTransactionsAsync()
        {
            try
            {
                if (!File.Exists(transactionsFilePath))
                {
                    // If the file does not exist, create it with an empty list
                    var emptyList = new List<Transaction>();
                    await SaveTransactionsAsync(emptyList);
                    return emptyList;
                }

                var json = await File.ReadAllTextAsync(transactionsFilePath);
                return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error loading transactions: {ex.Message}");
                throw; // Rethrow or handle as needed
            }
        }

        private async Task SaveTransactionsAsync(List<Transaction> transactions)
        {
            try
            {
                var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(transactionsFilePath, json);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error saving transactions: {ex.Message}");
                throw; // Rethrow or handle as needed
            }
        }
    }
}
*/