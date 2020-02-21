using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    class Checking : IAccount
    {
        public Checking(int actNumber, string owner, double balance)
        {
            Number = actNumber;
            Owner = owner;
            Type = "Checking";
            Balance = balance;
        }
        public int Number { get; set; }
        public string Owner { get; set; }
        public string Type { get; set; }
        public double Balance { get; set; }
    }
}
