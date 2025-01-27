using System;

namespace PersonalFinanceTracker.Models
{
    public class Transaction
    {
        public int UserId { get; set; } 
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Tag { get; set; }

    }

}

