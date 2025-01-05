
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Components.Pages
{
    public partial class Register
    {
        private User newUser = new User(); //obj created for User Class
        private string errorMessage;

        // New field for searching user
        private int searchUserId;

        // Field to hold retrieved user information
        private User retrievedUser;

        private async Task AddNewUser()
        {
            try
            {
                var existingUsers = await userService.LoadUsersAsync();
                // condition chek


                await userService.SaveUserAsync(newUser);
                // Reset the new user object after saving.
                newUser = new User();
                errorMessage = null; // Clear any previous error messages
                navigationManager.NavigateTo("/dashboard");
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred while adding the user: {ex.Message}";
                Console.WriteLine(errorMessage); // Log for debugging
            }
        }

        private async Task GetUserInfo()
        {
            try
            {
                var users = await userService.LoadUsersAsync();
                retrievedUser = users.FirstOrDefault(u => u.UserId == searchUserId);

                if (retrievedUser == null)
                {
                    errorMessage = "No user found with this ID.";
                }
                else
                {
                    errorMessage = null; // Clear any previous error messages
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred while retrieving the user: {ex.Message}";
                Console.WriteLine(errorMessage); // Log for debugging
            }
        }
    }
}