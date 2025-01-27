using Newtonsoft.Json;
using PersonalFinanceTracker.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class DebtService
{
    private readonly string debtsFilePath;

    public DebtService()
    {
        var baseDirectory = AppContext.BaseDirectory;
        debtsFilePath = Path.Combine(baseDirectory, "Data", "debts.json");
    }

    // Method to save debts to a file
    public async Task SaveDebtsAsync(List<Debt> newDebts)
    {
        // Load existing debts
        var existingDebts = await LoadDebtsAsync();
        var allDebts = existingDebts.Concat(newDebts).ToList();

        // Debugging: Log the debts to check
        Console.WriteLine(JsonConvert.SerializeObject(allDebts, Formatting.Indented));

        // Serialize and save the combined debts
        var json = JsonConvert.SerializeObject(allDebts, Formatting.Indented);
        await File.WriteAllTextAsync(debtsFilePath, json);
    }


    // Method to read debts from a file
    private async Task<List<Debt>> LoadDebtsAsync()
    {
        if (!File.Exists(debtsFilePath))
        {
            return new List<Debt>(); // Return an empty list if the file does not exist
        }

        var json = await File.ReadAllTextAsync(debtsFilePath);
        return JsonConvert.DeserializeObject<List<Debt>>(json) ?? new List<Debt>();
    }

    // Method to get all debts for a specific user
    public async Task<List<Debt>> GetDebtsAsync(int userId)
    {
        var debts = await LoadDebtsAsync();
        return debts.Where(d => d.UserId == userId).ToList();  // Filter by UserId
    }

    // Method to get all debts
    public async Task<List<Debt>> GetAllDebtsAsync()
    {
        return await LoadDebtsAsync();
    }
}
