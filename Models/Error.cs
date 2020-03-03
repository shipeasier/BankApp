using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Models
{
    class Error
    {
        public Error(string message, List<Transactions> transactions = null)
        {
            Message = message;
            if (transactions != null && transactions.Count > 0)
            {
                Transactions = transactions;
            }
        }
        public string Message { get; set; }
        public List<Transactions> Transactions { get; set; }

    }
}
