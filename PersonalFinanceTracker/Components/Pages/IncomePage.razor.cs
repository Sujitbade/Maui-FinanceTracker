using MudBlazor;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Services;


namespace PersonalFinanceTracker.Components.Pages
{
    public partial class IncomePage
    {
        private string searchString = "";
        private bool showForm;
        private DateRange? selectedDateRange;
        private List<Transaction> transactions = new List<Transaction>();
        private Transaction newTransaction = new Transaction();
        private string selectedTag;
        private decimal UserBalance { get; set; }
        private List<string> Tags = new List<string>();
         
        protected override async Task OnInitializedAsync()
        {
            // Load tags
            Tags = await tagService.ReadTagsAsync();

            // Get the authenticated user
            var currentUser = authStateService.GetAuthenticatedUser(); // Replace this with your actual method to get the user

            if (currentUser != null)
            {
                UserBalance = await userBalanceService.GetUserBalanceAsync(currentUser.UserId);
                // Load income transactions for the authenticated user
                transactions = await transactionService.GetIncomeTransactionsAsync(currentUser.UserId);
            }
            else
            {
                // Handle the case when the user is not authenticated
                Console.WriteLine("User not authenticated.");
            }
        }


        private decimal TotalIncomeAmount => transactions.Sum(transaction => transaction.Amount);
        private int TotalIncomeTransaction => transactions.Count(transaction => transaction.Type == "Income");

        private void OpenForm()
        {
            newTransaction = new Transaction();
            showForm = true;
        }

        private void CloseForm()
        {
            showForm = false;
        }

        private async Task AddTransaction()
        {
            if (string.IsNullOrWhiteSpace(newTransaction.Description) || newTransaction.Amount <= 0)
            {
                Console.WriteLine("Invalid transaction details.");
                return;
            }

            var currentUser = authStateService.GetAuthenticatedUser(); // Get the authenticated user
            if (currentUser != null)
            {
                // Create a new transaction and add it to the list
                transactions.Add(new Transaction
                {
                    UserId = currentUser.UserId,  // Assign the UserId
                    Description = newTransaction.Description,
                    Date = newTransaction.Date == default ? DateTime.Now : newTransaction.Date,
                    Amount = newTransaction.Amount,
                    Type = "Income",
                    Tag = selectedTag
                });

                // Save the transactions to the file
                await transactionService.SaveTransactionsAsync(transactions);

                    UserBalance += newTransaction.Amount;

                // Optionally update the balance in UserBalanceService
                await userBalanceService.UpdateUserBalanceAsync(currentUser.UserId, UserBalance);

                // Re-fetch transactions for the user to ensure the list is up-to-date
                transactions = await transactionService.GetIncomeTransactionsAsync(currentUser.UserId);

                CloseForm();
            }
            else
            {
                Console.WriteLine("User not authenticated.");
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
    }
}