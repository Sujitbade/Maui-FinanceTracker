using System;
using System;

namespace PersonalFinanceTracker.Models
{
    public class Transaction
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public List<string> ExpenseTags { get; set; } = new List<string>
{
    "Rent", "Utilities", "Internet", "Transportation", "Groceries",
    "Dining Out", "Entertainment", "Insurance", "Medical Bills",
    "Debt Repayment", "Shopping", "Subscription Services", "Pet Care",
    "Education", "Home Maintenance", "Taxes", "Childcare", "Gifts & Donations",
    "Travel", "Miscellaneous"
};

        public List<string> IncomeTags { get; set; } = new List<string>
{
    "Salary", "Freelance Work", "Business Revenue", "Dividends",
    "Rental Income", "Bonuses", "Commission", "Interest Income",
    "Gifts & Inheritances", "Government Benefits", "Scholarships",
    "Royalties", "Pension", "Refunds", "Side Hustles", "Passive Income",
    "Stocks/Crypto Gains", "Sales Income", "Crowdfunding", "Tips & Bonuses"
};

    }
}

