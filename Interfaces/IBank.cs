using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    interface IBank
    {
        public string Name { get; set; }

        public List<IAccount> Accounts { get; set; }

    }
}
