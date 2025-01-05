namespace PersonalFinanceTracker.Components.Pages
{
    public partial class Login
    {
        private int spacing = 4;
        private string username;
        private string password;
        private string errorMessage;

        private async Task LoginUser()
        {
            try
            {
                var users = await userService.LoadUsersAsync();
                var user = users.FirstOrDefault(u => u.Username == username && u.Password == HashPassword(password));

                if (user != null)
                {
                    // Set the authenticated user in state management
                    authStateService.SetAuthenticatedUser(user);

                    // Redirect to the dashboard or todo page
                    navigationManager.NavigateTo("/dashboard");
                }
                else
                {
                    errorMessage = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred during login: {ex.Message}";
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}