using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    class IndividualInvestment : IAccount
    {
        public IndividualInvestment(int actNumber, string owner, double balance)
        {
            Number = actNumber;
            Owner = owner;
            Balance = balance;
            Type = "IndividualInvestment";
        }
        public int Number { get; set; }
        public string Owner { get; set; }
        public string Type { get; set; }
        public double Balance { get; set; }
    }
}
