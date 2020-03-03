using BankApp.Models;
using Microsoft.ML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BankApp
{
    class AccountServices
    {
        public IAccount Deposit(double amount, IAccount account)
        {
            account.Balance += amount;
            return account;
        }

        public IAccount Withdrawal(double amount, IAccount account)
        {
            var orgBalance = account.Balance;
            account.Balance -= amount;
            if (account.Balance < 0)
            {
                account.Balance = orgBalance;
                throw new Exception("Overdrafting account balances is not permitted");
            }
            return account;
        }

        public IAccount Withdrawal(Transactions transactions)
        {
            var orgBalance = transactions.Account.Balance;
            transactions.Account.Balance -= Convert.ToDouble(transactions.Amount);
            if (transactions.Account.Balance < 0)
            {
                transactions.Account.Balance = orgBalance;
                throw new Exception("Overdrafting account balances is not permitted");
            }
            return transactions.Account;
        }

        public List<IAccount> Transfers(double amount, IAccount fromAccount, IAccount toAccount)
        {
            List<IAccount> accounts = new List<IAccount>();
            var orgFromBalance = fromAccount.Balance;
            var orgToBalance = toAccount.Balance;
            try
            {
                fromAccount = Withdrawal(amount, fromAccount);
                toAccount = Deposit(amount, toAccount);
            }
            catch (Exception ex)
            {
                fromAccount.Balance = orgFromBalance;
                toAccount.Balance = orgToBalance;
                throw ex;
            }
            accounts.Add(fromAccount);
            accounts.Add(toAccount);
            return accounts;

        }

        public StringBuilder GetAccountInfo(IAccount account)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Account Info").Append(Environment.NewLine)
                .Append("Owner: ").Append(account.Owner).Append(Environment.NewLine)
                .Append("Number: ").Append(account.Number).Append(Environment.NewLine)
                .Append("Type: ").Append(account.Type).Append(Environment.NewLine)
                .Append("Balance: ").Append(account.Balance);
            return sb;
        }

        //public IAccount AISpikeDetection(List<Transactions> transactionsList, IAccount account)
        //{
        //    MLContext mLContext = new MLContext();

        //    IDataView dataView = mLContext.Data.LoadFromEnumerable(transactionsList);

        //    // training algorithm
        //    var abWithdrawTrainer = mLContext.Transforms.DetectIidSpike(outputColumnName: nameof(TransactionPrediction.Prediction), inputColumnName: nameof(Transactions.Amount), confidence: 99, pvalueHistoryLength: (36 / 4));

        //    // create the transform, we need an empty dataset first
        //    IEnumerable<Transactions> enumerableData = new List<Transactions>();
        //    ITransformer transformer = abWithdrawTrainer.Fit((mLContext.Data.LoadFromEnumerable(enumerableData)));

        //    // apply data transformation to create prediction
        //    IDataView transformedData = transformer.Transform(dataView);

        //    var predictions = mLContext.Data.CreateEnumerable<TransactionPrediction>(transformedData, reuseRowObject: false);

        //    Console.WriteLine("Alert\tScore\tP-Value");

        //    foreach (var item in predictions)
        //    {

        //        var results = $"{item.Prediction[0]}\t{item.Prediction[1]:f2}\t{item.Prediction[2]:F2}";
        //        if (item.Prediction[0] == 1)
        //        {
                    
        //            results += " <-- Spike detected, do not withdraw";
        //        }
        //        else
        //        {
        //            // do the withdraw
        //        }
        //        //Console.WriteLine(item.Prediction[0]);
        //        //Console.WriteLine(val);
        //        Console.WriteLine(results);
        //    }


        //    return null;
        //}
    }
}
