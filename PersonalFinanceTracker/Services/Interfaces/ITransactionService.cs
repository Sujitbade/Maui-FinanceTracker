using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Services
{
    public interface ITransactionService
    {

        Task AddTransactionAsync(Transaction task);


        Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
    }
}
