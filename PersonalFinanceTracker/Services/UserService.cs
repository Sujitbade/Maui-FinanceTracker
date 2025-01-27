using PersonalFinanceTracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Services
{
    public class UserService : IUserService
    {
        private readonly string usersFilePath = Path.Combine(AppContext.BaseDirectory, "Data","UserDetails.json");

        // Registers a new user
        public async Task<bool> RegisterUserAsync(User newUser)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.Password))
                {
                    Console.WriteLine("Username and password are required.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(newUser.PreferredCurrency))
                {
                    Console.WriteLine("Preferred currency is required.");
                    return false;
                }

                var users = await LoadUsersAsync();

                // Check for duplicate username
                if (users.Exists(u => u.Username.Equals(newUser.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Username is already taken.");
                    return false;
                }

                // Hash the password
                newUser.Password = HashPassword(newUser.Password);

                // Assign a unique UserId
                newUser.UserId = users.Count > 0 ? users[^1].UserId + 1 : 1;

                // Set initial balance
                newUser.Balance = 0.0m;

                // Save the new user
                users.Add(newUser);
                await SaveUsersAsync(users);

                Console.WriteLine("User registered successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during registration: {ex.Message}");
                return false;
            }
        }

        // Loads users from the JSON file
        public async Task<List<User>> LoadUsersAsync()
        {
            try
            {
                if (!File.Exists(usersFilePath))
                {
                    return new List<User>();
                }

                var json = await File.ReadAllTextAsync(usersFilePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing user data: {ex.Message}");
                return new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                return new List<User>();
            }
        }

        // Saves users to the JSON file
        private async Task SaveUsersAsync(List<User> users)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(users, options);
                await File.WriteAllTextAsync(usersFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user data: {ex.Message}");
                throw;
            }
        }

        // Hashes a password using SHA-256
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Verifies the username and password during login
        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            try
            {
                var users = await LoadUsersAsync();
                var hashedPassword = HashPassword(password);

                // Find the user with matching credentials
                var user = users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == hashedPassword);

                if (user == null)
                {
                    Console.WriteLine("Invalid username or password.");
                    return null;
                }

                Console.WriteLine("User authenticated successfully.");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during authentication: {ex.Message}");
                return null;
            }
        }

        public Task SaveUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
