using BankApp.Models;
using BankApp.Services;
using System;
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
            DepositExample();
            Console.WriteLine("==================================");
            Console.WriteLine(Environment.NewLine);
            WithDrawExample();
            Console.WriteLine("==================================");
            Console.WriteLine(Environment.NewLine);
            WithDrawInvidualInvestmentExample();
            Console.WriteLine("==================================");
            Console.WriteLine(Environment.NewLine);
            WithDrawOverDraftExample();
            Console.WriteLine("==================================");
            Console.WriteLine(Environment.NewLine);
            TransferAccountExample();
            Console.WriteLine("==================================");
            Console.WriteLine(Environment.NewLine);
            Console.ReadLine();
        }
        public static Bank InitBankOne()
        {
            List<IAccount> bankOneAccounts = new List<IAccount>();
            Checking checkingActOne = new Checking(11111222, "John Smith", 2000.00);
            CorporateInvestment corporateInvestmentActOne = new CorporateInvestment(11111333, "Indiana Interactive", 30050.25);
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
                    bankOne.Accounts[i] = accountServices.Deposit(500.00, bankOne.Accounts[i]);
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
                        bankOne.Accounts[i] = accountServices.Withdrawal(500.00, bankOne.Accounts[i]);
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
    }

}
