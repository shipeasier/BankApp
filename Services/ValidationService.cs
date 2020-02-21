using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Services
{
    class ValidationService
    {
        public ValidationService()
        {
            Errors = new List<Error>();
        }
        
        public List<Error> Errors = new List<Error>();

        public List<Error> IsValid(double amount, IAccount account)
        {
            List<Error> list = new List<Error>();

            if (account.Type == "IndividualInvestment" && amount > 1000.00)
            {
                list.Add(new Error("Error: Individual Investment Accounts cannot withdraw more than $1,000 in a given transaction"));
            }

            return list;
        }
    }
}

