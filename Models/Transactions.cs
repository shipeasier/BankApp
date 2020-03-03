using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Models
{
    class Transactions
    {
        public Transactions(string day, float amount, IAccount account)
        {
            Account = account;
            Day = day;
            Amount = amount;
        }

        [NoColumn]
        public IAccount Account { get; set; }
        [LoadColumn(0)]
        public string Day { get; set; }
        [LoadColumn(1)]
        public float Amount { get; set; }
    }

    class TransactionPrediction
    {
        //vector to hold alert,score,p-value values
        [VectorType(3)]
        public double[] Prediction { get; set; }
    }

}
