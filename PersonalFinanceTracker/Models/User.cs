﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Models
{
    public class User
    {
        public int UserId { get; set; }
        public  string Username { get; set; }
        public  string Password { get; set; }
        public  string PreferredCurrency { get; set; }
        public decimal Balance { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public ICollection<Debt> Debts { get; set; }

    }

}


