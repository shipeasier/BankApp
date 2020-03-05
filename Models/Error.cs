using BankApp.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Models
{
    class Error
    {
        public Error(ErrorNumber errorNumber, string message, List<Transactions> transactions = null)
        {
            ErrorNumber = (long)errorNumber;
            Message = message;
            if (transactions != null && transactions.Count > 0)
            {
                Transactions = transactions;
            }
        }
        public string Message { get; set; }
        public List<Transactions> Transactions { get; set; }
        public long ErrorNumber { get; set; }
    }
}
