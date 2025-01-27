using Newtonsoft.Json;
using PersonalFinanceTracker.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class UserBalanceService
{
    private readonly string userBalanceFilePath;

    public UserBalanceService()
    {
        var baseDirectory = AppContext.BaseDirectory;
        userBalanceFilePath = Path.Combine(baseDirectory, "Data", "userBalances.json");
    }

    // Method to get the balance of a user by UserId
    public async Task<decimal> GetUserBalanceAsync(int userId)
    {
        var balances = await LoadUserBalancesAsync();
        var userBalance = balances.FirstOrDefault(b => b.UserId == userId);

        return userBalance?.Balance ?? 0; // Return 0 if balance not found
    }

    // Method to update the balance of a user
    public async Task UpdateUserBalanceAsync(int userId, decimal newBalance)
    {
        var balances = await LoadUserBalancesAsync();
        var userBalance = balances.FirstOrDefault(b => b.UserId == userId);

        if (userBalance != null)
        {
            userBalance.Balance = newBalance;
        }
        else
        {
            // If user balance does not exist, create a new one
            balances.Add(new UserBalance { UserId = userId, Balance = newBalance });
        }

        await SaveUserBalancesAsync(balances);
    }

    // Method to load all user balances from the file
    private async Task<List<UserBalance>> LoadUserBalancesAsync()
    {
        if (!File.Exists(userBalanceFilePath))
        {
            return new List<UserBalance>(); // Return an empty list if no data
        }

        var json = await File.ReadAllTextAsync(userBalanceFilePath);
        return JsonConvert.DeserializeObject<List<UserBalance>>(json) ?? new List<UserBalance>();
    }

    // Method to save all user balances to the file
    private async Task SaveUserBalancesAsync(List<UserBalance> userBalances)
    {
        var json = JsonConvert.SerializeObject(userBalances, Formatting.Indented);

        // Ensure the directory exists before saving the file
        var directory = Path.GetDirectoryName(userBalanceFilePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(userBalanceFilePath, json);
    }
}
