using System.Collections.Generic;
using System.Threading.Tasks;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(User user);
        Task<User?> AuthenticateUserAsync(string username, string password);
        Task<List<User>> LoadUsersAsync();
        Task SaveUserAsync(User user);
    }
}
