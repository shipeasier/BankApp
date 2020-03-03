using BankApp.Models;
using Microsoft.ML;
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

       public List<Error> IsValid(double? amount, IAccount account, List<Transactions> transactionsList = null)
       // public List<Error> IsValid(double amount, IAccount account)
        {
            List<Error> list = new List<Error>();

            if (account.Type == "IndividualInvestment" && amount > 1000.00)
            {
                list.Add(new Error("Error: Individual Investment Accounts cannot withdraw more than $1,000 in a given transaction"));
            }

            if (transactionsList != null && transactionsList.Count > 0 && amount == null)
            {
                List<Transactions> transactions = AISpikeDetection(transactionsList, account);
                if (transactions.Count > 0)
                {
                    foreach (var item in transactions)
                    {
                        list.Add(new Error("Error: Transcation Spike Detected " + item.Day + " " + item.Amount, transactions));
                    }
                    
                    // withdraw, else throw an error 
                    // I think what we need to do is return the list of good transcations and then make em

                }
            }

            return list;
        }

        public List<Transactions> AISpikeDetection(List<Transactions> transactionsList, IAccount account)
        {
            MLContext mLContext = new MLContext();
            List<Transactions> invalidTransactions = new List<Transactions>();
            IDataView dataView = mLContext.Data.LoadFromEnumerable(transactionsList);

            // training algorithm
            var abWithdrawTrainer = mLContext.Transforms.DetectIidSpike(outputColumnName: nameof(TransactionPrediction.Prediction), inputColumnName: nameof(Transactions.Amount), confidence: 99, pvalueHistoryLength: (36 / 4));

            // create the transform, we need an empty dataset first
            IEnumerable<Transactions> enumerableData = new List<Transactions>();
            ITransformer transformer = abWithdrawTrainer.Fit((mLContext.Data.LoadFromEnumerable(enumerableData)));

            // apply data transformation to create prediction
            IDataView transformedData = transformer.Transform(dataView);

            var predictions = mLContext.Data.CreateEnumerable<TransactionPrediction>(transformedData, reuseRowObject: false);

         //   Console.WriteLine("Alert\tScore\tP-Value");

            foreach (var item in predictions)
            {
             //   var results = $"{item.Prediction[0]}\t{item.Prediction[1]:f2}\t{item.Prediction[2]:F2}";
                if (item.Prediction[0] == 1) // if alert is one
                {
                    invalidTransactions.Add(transactionsList.Find(r => r.Amount == item.Prediction[1]));
                }
                //Console.WriteLine(results);
            }
            return invalidTransactions;
        }

        public List<Transactions> RemoveInvalidTransactions(List<Transactions> transactions, List<Transactions> invalidTransactions)
        {
            Console.WriteLine(transactions.Count);

            // remove the invalid transactions // this should be a method
            // then we'll do the withdraw
            if (invalidTransactions.Count > 0)
            {
                foreach (var item in invalidTransactions)
                {
                    Console.Write("removed invalid transaction ");
                    Console.WriteLine(item.Day + " " + item.Amount);
                    transactions.RemoveAll(r => r.Amount == item.Amount);
                }
            }
            return transactions;
        }
    }
}

