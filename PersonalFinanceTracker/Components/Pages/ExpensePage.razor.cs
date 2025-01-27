using MudBlazor;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Services;

namespace PersonalFinanceTracker.Components.Pages
{
    public partial class ExpensePage
    {
        private string searchString = "";
        private bool showForm;
        private DateRange? selectedDateRange;
        private List<Transaction> transactions = new List<Transaction>();
        private Transaction newTransaction = new Transaction();
        private string selectedTag;
        private decimal UserBalance { get; set; }
        private List<string> Tags = new List<string>();

        // Alert component properties
        private bool showAlert;
        private string alertMessage;
        private Severity alertSeverity;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Load tags
                Tags = await tagService.ReadTagsAsync();

                // Get the authenticated user
                var currentUser = authStateService.GetAuthenticatedUser(); // Replace this with your actual method to get the user

                if (currentUser != null)
                {
                    UserBalance = await userBalanceService.GetUserBalanceAsync(currentUser.UserId);

                    // Load expense transactions for the authenticated user
                    transactions = await transactionService.GetExpenseTransactionsAsync(currentUser.UserId);
                }
                else
                {
                    ShowAlert("User not authenticated.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Error during initialization: {ex.Message}", Severity.Error);
            }
        }

        private decimal TotalExpenseAmount => transactions.Sum(transaction => transaction.Amount);
        private int TotalExpenseTransaction => transactions.Count(transaction => transaction.Type == "Expense");

        private void OpenForm()
        {
            newTransaction = new Transaction(); // Reset form data
            selectedTag = null;                 // Reset tag selection
            showForm = true;
        }

        private void CloseForm()
        {
            showForm = false;
        }

        private async Task AddTransaction()
        {
            // Validate transaction details
            if (string.IsNullOrWhiteSpace(newTransaction.Description) || newTransaction.Amount <= 0)
            {
                ShowAlert("Invalid transaction details. Please enter a valid description and amount.", Severity.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedTag))
            {
                ShowAlert("Please select a tag for the transaction.", Severity.Warning);
                return;
            }

            // Get the authenticated user
            var currentUser = authStateService.GetAuthenticatedUser();

            if (currentUser != null)
            {
                try
                {
                    newTransaction.Type = "Expense"; // Set the type to Expense

                    // Add a new transaction
                    transactions.Add(new Transaction
                    {
                        UserId = currentUser.UserId,  // Assign the UserId
                        Description = newTransaction.Description,
                        Date = newTransaction.Date == default ? DateTime.UtcNow : newTransaction.Date,
                        Amount = newTransaction.Amount,
                        Type = newTransaction.Type,
                        Tag = selectedTag
                    });

                    // Save the transactions to the file
                    await transactionService.SaveTransactionsAsync(transactions);

                    // Deduct the amount from UserBalance
                    UserBalance -= newTransaction.Amount;

                    // Update the balance in UserBalanceService
                    await userBalanceService.UpdateUserBalanceAsync(currentUser.UserId, UserBalance);

                    // Re-fetch transactions for the user to ensure the list is up-to-date
                    transactions = await transactionService.GetExpenseTransactionsAsync(currentUser.UserId);

                    ShowAlert("Transaction added successfully.", Severity.Success);

                    // Close the form or reset the input fields
                    CloseForm();
                }
                catch (Exception ex)
                {
                    ShowAlert($"Error adding transaction: {ex.Message}", Severity.Error);
                }
            }
            else
            {
                ShowAlert("User not authenticated.", Severity.Error);
            }
        }

        private bool FilterFunc(Transaction transaction)
        {
            // Title filter
            if (!string.IsNullOrWhiteSpace(searchString) &&
                !transaction.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Date range filter
            if (selectedDateRange != null)
            {
                var startDate = selectedDateRange.Start;
                var endDate = selectedDateRange.End;

                if (startDate.HasValue && endDate.HasValue)
                {
                    if (transaction.Date < startDate.Value || transaction.Date > endDate.Value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void ClearDateRange()
        {
            selectedDateRange = null;
        }

        private void ShowAlert(string message, Severity severity)
        {
            alertMessage = message;
            alertSeverity = severity;
            showAlert = true;

            // Auto-hide alert after a few seconds (optional)
            _ = Task.Delay(3000).ContinueWith(_ => showAlert = false);
        }
    }
}
