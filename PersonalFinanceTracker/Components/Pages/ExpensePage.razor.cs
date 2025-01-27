using MudBlazor;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Services;

namespace PersonalFinanceTracker.Components.Pages
{
    public partial class ExpensePage
    {
        // State and Data Variables
        private string searchString = "";
        private bool showForm;
        private DateRange? selectedDateRange;
        private List<Transaction> transactions = new List<Transaction>();
        private Transaction newTransaction = new Transaction();
        private string selectedTag;

        private decimal UserBalance;
        private List<string> Tags = new List<string>();

        // Alert Properties
        private bool showAlert;
        private string alertMessage;
        private Severity alertSeverity;

        // Computed Properties
        private decimal TotalExpenseAmount => transactions.Sum(transaction => transaction.Amount);
        private int TotalExpenseTransaction => transactions.Count;

        /// <summary>
        /// Initializes data on component load.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Tags = await tagService.ReadTagsAsync();
                var currentUser = authStateService.GetAuthenticatedUser();

                if (currentUser != null)
                {
                    var incomeTransactions = await transactionService.GetIncomeTransactionsAsync(currentUser.UserId);
                    var expenseTransactions = await transactionService.GetExpenseTransactionsAsync(currentUser.UserId);

                    UserBalance = incomeTransactions.Sum(t => t.Amount) - expenseTransactions.Sum(t => t.Amount);
                    transactions = expenseTransactions;
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

        /// <summary>
        /// Opens the Add Transaction form.
        /// </summary>
        private void OpenForm() => showForm = true;

        /// <summary>
        /// Closes the Add Transaction form.
        /// </summary>
        private void CloseForm() => showForm = false;

        /// <summary>
        /// Handles adding a new expense transaction.
        /// </summary>
        private async Task AddTransaction()
        {
            if (string.IsNullOrWhiteSpace(newTransaction.Description) || newTransaction.Amount <= 0 ||
                string.IsNullOrWhiteSpace(selectedTag))
            {
                ShowAlert("Invalid details. Please fill out all fields.", Severity.Warning);
                return;
            }

            // Check if the expense exceeds the user's balance
            if (newTransaction.Amount > UserBalance)
            {
                ShowAlert("Insufficient balance to add this transaction.", Severity.Warning);
                return;
            }

            try
            {
                var currentUser = authStateService.GetAuthenticatedUser();
                if (currentUser != null)
                {
                    transactions.Add(new Transaction
                    {
                        UserId = currentUser.UserId,
                        Description = newTransaction.Description,
                        Date = newTransaction.Date == default ? DateTime.UtcNow : newTransaction.Date,
                        Amount = newTransaction.Amount,
                        Type = "Expense",
                        Tag = selectedTag
                    });

                    // Update the user's balance after adding the transaction
                    UserBalance -= newTransaction.Amount;

                    await transactionService.SaveTransactionsAsync(transactions);
                    CloseForm();
                    ShowAlert("Transaction added successfully.", Severity.Success);
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Error adding transaction: {ex.Message}", Severity.Error);
            }
        }


        /// <summary>
        /// Filters transactions based on the search input and selected date range.
        /// </summary>
        private bool FilterFunc(Transaction transaction)
        {
            if (!string.IsNullOrWhiteSpace(searchString) &&
                !transaction.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (selectedDateRange != null &&
                (transaction.Date < selectedDateRange.Start || transaction.Date > selectedDateRange.End))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Resets the selected date range filter.
        /// </summary>
        private void ClearDateRange() => selectedDateRange = null;

        /// <summary>
        /// Displays an alert with a message and severity level.
        /// </summary>
        private void ShowAlert(string message, Severity severity)
        {
            alertMessage = message;
            alertSeverity = severity;
            showAlert = true;
        }
    }
}
