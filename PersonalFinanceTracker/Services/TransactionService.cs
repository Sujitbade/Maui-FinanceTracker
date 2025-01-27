using Newtonsoft.Json;
using PersonalFinanceTracker.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


public class TransactionService
{
    private readonly string transactionsFilePath;

    public TransactionService()
    {
        var baseDirectory = AppContext.BaseDirectory;
        transactionsFilePath = Path.Combine(baseDirectory, "Data", "transactions.json");
    }

    // Method to save transactions to a file
    public async Task SaveTransactionsAsync(List<Transaction> newUserTransactions)
    {
        // Load the existing transactions from the file
        var existingTransactions = await LoadTransactionsAsync();

        // Append the new user transactions to the existing list
        var allTransactions = existingTransactions.Concat(newUserTransactions).ToList();

        // Serialize the combined list of transactions into JSON format
        var json = JsonConvert.SerializeObject(allTransactions, Formatting.Indented);

        // Ensure the directory exists before saving the file
        var directory = Path.GetDirectoryName(transactionsFilePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Write the combined transactions list back to the file
        await File.WriteAllTextAsync(transactionsFilePath, json);
    }


    // Method to read transactions from a file
    private async Task<List<Transaction>> LoadTransactionsAsync()
    {
        if (!File.Exists(transactionsFilePath))
        {
            return new List<Transaction>(); // Return an empty list if file does not exist
        }

        var json = await File.ReadAllTextAsync(transactionsFilePath);
        return JsonConvert.DeserializeObject<List<Transaction>>(json) ?? new List<Transaction>();
    }

    // Method to get all transactions for a user
    public async Task<List<Transaction>> GetTransactionsAsync(int userId)
    {
        var transactions = await LoadTransactionsAsync();
        return transactions.Where(t => t.UserId == userId).ToList();  // Filter by UserId
    }



    // Method to get income transactions for a user
    public async Task<List<Transaction>> GetIncomeTransactionsAsync(int userId)
    {
        var transactions = await GetTransactionsAsync(userId);  // Get all transactions for the user
        return transactions.Where(t => t.Type!=null && t.Type.Equals("Income", StringComparison.OrdinalIgnoreCase)).ToList();
    }

    // Method to get expense transactions for a user
    public async Task<List<Transaction>> GetExpenseTransactionsAsync(int userId)
    {
        var transactions = await GetTransactionsAsync(userId);  // Get all transactions for the user

        // Filter transactions by type (Expense), ensuring Type is not null
        return transactions.Where(t => t.Type != null && t.Type.Equals("Expense", StringComparison.OrdinalIgnoreCase)).ToList();
    }


}
