using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    interface IAccount
    {
        public int Number { get; set; }
        public string Owner { get; set; }
        public string Type { get; set; }
        public double Balance { get; set; }

    }
}
