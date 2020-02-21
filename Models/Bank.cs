using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    class Bank : IBank
    {
        public Bank(string name, List<IAccount> accounts)
        {
            Name = name;
            Accounts = accounts;
        }
        public string Name { get; set; }
        public List<IAccount> Accounts { get; set; }
    }
}
