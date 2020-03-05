using BankApp.Models;
using BankApp.Services;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BankApp
{
    class Program
    {
        // one bank, multiple accounts
        static ValidationService validationService = new ValidationService();
        static Bank bankOne = InitBankOne();
        static AccountServices accountServices = new AccountServices();
        static void Main(string[] args)
        {
            DisplayAllAccount();
            Console.WriteLine("==================================");
            Console.WriteLine(Environment.NewLine);
         //   DepositExample();
         //   Console.WriteLine("==================================");
         //   Console.WriteLine(Environment.NewLine);
            WithDrawExample();
            Console.WriteLine("==================================");
            Console.WriteLine(Environment.NewLine);
         //   WithDrawInvidualInvestmentExample();
         //   Console.WriteLine("==================================");
         //   Console.WriteLine(Environment.NewLine);
         //   WithDrawOverDraftExample();
         //   Console.WriteLine("==================================");
         //   Console.WriteLine(Environment.NewLine);
         //   TransferAccountExample();
         //   Console.WriteLine("==================================");
         //   Console.WriteLine(Environment.NewLine);
            //AIWithdrawAnomalyDetectionExample();
            //Console.WriteLine("==================================");
            //Console.WriteLine(Environment.NewLine);


            Console.ReadLine();



        }

        public static void AIWithdrawAnomalyDetectionExample()
        {
            Console.WriteLine("Withdraw Example with AI assistance" + Environment.NewLine);
            AccountServices accountServices = new AccountServices();
            CorporateInvestment corporateInvestmentActOne = new CorporateInvestment(11111333, "Indiana State", 30050.25);
            // so i can do a bunch of withdraws and look for somethign goofy
            // i think it would be cool to have something wiht transfering account
            
            // find john smith
            var johnChkAct = bankOne.Accounts.Find(r => r.Number == 11111222);
            // lets give John Smith 35 grand
            accountServices.Deposit(35000.00, johnChkAct);

            var list = GetTransactionsList(johnChkAct);
            List<Error> errList = (validationService.IsValid(null, johnChkAct, list));
            if (errList.Count > 0)
            {
                foreach (var item in errList)
                {
                    Console.WriteLine(item.Message);
                    // clean up the invalids
                    list = validationService.RemoveInvalidTransactions(list, item.Transactions);
                    break;
                }
            }
            // then we can do the withdraw :) :) :) :) 

            foreach (var item in list)
            {
                // we need to change this  to look at transcation object
                johnChkAct = accountServices.Withdrawal(item);
                Console.WriteLine(accountServices.GetAccountInfo(johnChkAct));
                Console.Write(Environment.NewLine);

            }
        }


        public static Bank InitBankOne()
        {
            List<IAccount> bankOneAccounts = new List<IAccount>();
            Checking checkingActOne = new Checking(11111222, "John Smith", 2000.00);
            CorporateInvestment corporateInvestmentActOne = new CorporateInvestment(11111333, "Indiana State", 30050.25);
            IndividualInvestment individualInvestmentActOne = new IndividualInvestment(11111444, "John Smith", 800.00);

            bankOneAccounts.Add(checkingActOne);
            bankOneAccounts.Add(corporateInvestmentActOne);
            bankOneAccounts.Add(individualInvestmentActOne);

            Bank bankOne = new Bank("Fifth Third Bank", bankOneAccounts);

            return bankOne;
        }

        public static void DisplayAllAccount()
        {
            Console.WriteLine("All Account Info" + Environment.NewLine);
            foreach (var item in bankOne.Accounts)
            {
                Console.WriteLine(accountServices.GetAccountInfo(item));
                Console.Write(Environment.NewLine);
            }
        }

        public static void DepositExample()
        {
            Console.WriteLine("Deposit Example" + Environment.NewLine);
            for (int i = 0; i < bankOne.Accounts.Count; i++)
            {
                if (bankOne.Accounts[i].Number == 11111222)
                {
                    //bankOne.Accounts[i] = accountServices.Deposit(500.00, bankOne.Accounts[i]);
                    bankOne.Accounts[i] = accountServices.Deposit(new Transactions(500.00f, bankOne.Accounts[i]));
                    Console.WriteLine(accountServices.GetAccountInfo(bankOne.Accounts[i]));
                    Console.Write(Environment.NewLine);
                }
            }
        }

        public static void WithDrawExample()
        {
            Console.WriteLine("Withdraw Example" + Environment.NewLine);
            for (int i = 0; i < bankOne.Accounts.Count; i++)
            {
                List<Error> errList = (validationService.IsValid(500.00, bankOne.Accounts[i]));
                if (errList.Count > 0)
                {
                    foreach (var item in errList)
                    {
                        Console.WriteLine(item.Message);
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine(accountServices.GetAccountInfo(bankOne.Accounts[i]));
                    }
                }
                else
                {
                    if (bankOne.Accounts[i].Number == 11111333)
                    {
                        //bankOne.Accounts[i] = accountServices.Withdrawal(500.00, bankOne.Accounts[i]);
                        bankOne.Accounts[i] = accountServices.Withdrawal(new Transactions(500.00f, bankOne.Accounts[i]));
                        Console.WriteLine(accountServices.GetAccountInfo(bankOne.Accounts[i]));
                        Console.Write(Environment.NewLine);
                    }
                }
            }
        }

        public static void WithDrawInvidualInvestmentExample()
        {
            Console.WriteLine("Individual Investment Account Example With Overdraft Exception" + Environment.NewLine);
            for (int i = 0; i < bankOne.Accounts.Count; i++)
            {
                if (bankOne.Accounts[i].Number == 11111444)
                {
                    List<Error> errList = (validationService.IsValid(2000.00, bankOne.Accounts[i]));
                    if (errList.Count > 0)
                    {
                        foreach (var item in errList)
                        {
                            Console.WriteLine(item.Message);
                            Console.WriteLine(Environment.NewLine);
                            Console.WriteLine(accountServices.GetAccountInfo(bankOne.Accounts[i]));
                        }
                    }
                    else
                    {
                        try
                        {
                            bankOne.Accounts[i] = accountServices.Withdrawal(2000.00, bankOne.Accounts[i]);
                            Console.WriteLine(accountServices.GetAccountInfo(bankOne.Accounts[i]));
                            Console.Write(Environment.NewLine);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + Environment.NewLine);
                            Console.WriteLine(accountServices.GetAccountInfo(bankOne.Accounts[i]));
                            Console.Write(Environment.NewLine);
                        }
                    }
                }
            }
        }
        public static void WithDrawOverDraftExample()
        {
            Console.WriteLine("Account Withdrawal With Overdraft Exception" + Environment.NewLine);
            for (int i = 0; i < bankOne.Accounts.Count; i++)
            {
                if (bankOne.Accounts[i].Number == 11111444)
                {
                    List<Error> errList = (validationService.IsValid(900.00, bankOne.Accounts[i]));
                    if (errList.Count > 0)
                    {
                        foreach (var item in errList)
                        {
                            Console.WriteLine(item.Message);
                            Console.WriteLine(Environment.NewLine);
                            Console.WriteLine(accountServices.GetAccountInfo(bankOne.Accounts[i]));
                        }
                    }
                    else
                    {
                        try
                        {
                            bankOne.Accounts[i] = accountServices.Withdrawal(900.00, bankOne.Accounts[i]);
                            Console.WriteLine(accountServices.GetAccountInfo(bankOne.Accounts[i]));
                            Console.Write(Environment.NewLine);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + Environment.NewLine);
                            Console.WriteLine(accountServices.GetAccountInfo(bankOne.Accounts[i]));
                            Console.Write(Environment.NewLine);
                        }
                    }
                }
            }
        }

        public static void TransferAccountExample()
        {
            var fromAccount = bankOne.Accounts.Find(r => r.Number == 11111444);
            var toAccount = bankOne.Accounts.Find(r => r.Number == 11111333);

            Console.WriteLine("Transfer Account Balance Example" + Environment.NewLine);
                List<Error> errList = (validationService.IsValid(100.00, fromAccount));
                if (errList.Count > 0)
                {
                    foreach (var item in errList)
                    {
                        Console.WriteLine(item.Message);
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine(accountServices.GetAccountInfo(fromAccount));
                    }
                }
                else
                {
                    try
                    {
                        List<IAccount> accounts = accountServices.Transfers(100.00, fromAccount, toAccount);
                        foreach (var item in accounts)
                        {
                            Console.WriteLine(accountServices.GetAccountInfo(item));
                            Console.Write(Environment.NewLine);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine);
                        Console.WriteLine(accountServices.GetAccountInfo(fromAccount));
                        Console.Write(Environment.NewLine);
                    }
                }
            
        }

        static List<Transactions> GetTransactionsList(IAccount account)
        {
            List<Transactions> transactionsList = new List<Transactions>();
            transactionsList.Add(new Transactions("1 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("2 - Jan", 275.60f, account));
            transactionsList.Add(new Transactions("3 - Jan", 272.50f, account));
            transactionsList.Add(new Transactions("4 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("5 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("6 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("7 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("8 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("9 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("10 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("11 - Jan", 272.50f, account));
            transactionsList.Add(new Transactions("12 - Jan", 277.50f, account));
            transactionsList.Add(new Transactions("13 - Jan", 251.50f, account));
            transactionsList.Add(new Transactions("14 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("15 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("16 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("17 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("18 - Jan", 279.50f, account));
            transactionsList.Add(new Transactions("19 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("20 - Jan", 278.50f, account));
            transactionsList.Add(new Transactions("21 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("22 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("23 - Jan", 500.50f, account));
            transactionsList.Add(new Transactions("24 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("25 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("26 - Jan", 273.80f, account));
            transactionsList.Add(new Transactions("27 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("28 - Jan", 274.60f, account));
            transactionsList.Add(new Transactions("29 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("30 - Jan", 271.50f, account));
            transactionsList.Add(new Transactions("31 - Jan", 271.50f, account));
            return transactionsList;
        }
    }
}
