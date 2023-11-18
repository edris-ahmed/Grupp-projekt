using BankBootstrap.Data;
using BankBootstrap.Models;
using BankBootstrap.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankBootstrap
{
    internal static class UserFunctions 
    {
        private static User currentUser;
        private static BankContext currentContext;
        private static void PerfromUserChoices(User user, BankContext context, string choice)
        {
            currentUser = user;
            currentContext = context;

            while (choice != "6")
            {
                DisplayOptions(choice);

                switch (choice)
                {
                    case "1":
                        ShowAccountBalance();
                        break;

                    case "2":
                        TransferBetweenAccounts();
                        break;

                    case "3":
                        WithdrawFromAccount();
                        break;

                    case "4":
                        DepositToAccount();
                        break;

                    case "5":
                        OpenNewAccount();
                        break;
                        
                    case "6":
                        LogOut();
                        break;
                }          
            }
        }

        private static void DisplayOptions(string choice)
        {
            bool exitLoop = false;

            Console.WriteLine($"[1] See your accounts and balance");
            Console.WriteLine($"[2] Transfer money between accounts");
            Console.WriteLine($"[3] Withdraw");
            Console.WriteLine($"[4] Deposit");
            Console.WriteLine($"[5] Open new account");
            Console.WriteLine($"[6] Log out");

            Console.Write("Enter an option: ");
            
            while (!exitLoop)
            {
                choice = Console.ReadLine();

                if (choice != "1" && choice != "2" && choice != "3" && choice != "4" && choice != "5" && choice != "6")
                {
                    Console.WriteLine("Enter an option using [1] [2] [3] [4] [5] [6]");
                }
                else
                {
                    exitLoop = true;
                }
            }
        }

        private static void ShowAccountBalance()
        {
            Console.WriteLine("Your accounts and balances:");
            foreach (var account in currentUser.Accounts)
            {
                Console.WriteLine($"Account: {account.Name}, Balance: {account.Balance}");
            }
        }

        private static void TransferBetweenAccounts()
        {
            Console.Write("Choose the account to take money from enter [Name] or [Id]: ");
            string input1 = Console.ReadLine();
            Console.Write("Choose the account to transfer money to enter [Name] or [Id]: ");
            string input2 = Console.ReadLine();

            var selectedAccounts1 = currentUser.Accounts.FirstOrDefault(a => a.Name == input1 || a.Id.ToString() == input1);
            var selectedAccounts2 = currentUser.Accounts.FirstOrDefault(a => a.Name == input2 || a.Id.ToString() == input2);

            if (selectedAccounts1 != null && selectedAccounts2 != null)
            {
                Console.WriteLine("Enter the transfer amount: ");

                if (double.TryParse(Console.ReadLine(), out double transferAmount))
                {
                    if (selectedAccounts1.Balance >= transferAmount)
                    {
                        selectedAccounts1.Balance -= transferAmount;
                        selectedAccounts2.Balance += transferAmount;
                        currentContext.SaveChanges();
                        Console.WriteLine($"Transfer successfull. Updated balance for account {selectedAccounts2.Name} is {selectedAccounts2.Balance}");
                    }
                    else
                    {
                        Console.WriteLine("Insufficent funds.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
            else
            {
                Console.WriteLine("One or both of the accounts did not match the criteria.");
            }
        }

        private static void WithdrawFromAccount()
        {
            Console.WriteLine("Select the account to withdraw from (enter the account [Name] or [Id]");
            string input1 = Console.ReadLine();

            var selectedAccount = currentUser.Accounts.FirstOrDefault(a => a.Name == input1 || a.Id.ToString() == input1);

            if (selectedAccount != null)
            {
                Console.Write("Enter the amount to withdraw: ");

                if (double.TryParse(Console.ReadLine(), out double withdrawAmount))
                {
                    if (selectedAccount.Balance >= withdrawAmount)
                    {
                        selectedAccount.Balance -= withdrawAmount;
                        currentContext.SaveChanges();
                        Console.WriteLine($"Withdrawal successfull. Updated balance: {selectedAccount.Balance}");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient funds.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
            else
            {
                Console.WriteLine("No account matching the criteria found.");
            }
        }

        private static void DepositToAccount()
        {
            Console.WriteLine("Select the account to deposit into (enter the account [Name] or [Id]");
            string input1 = Console.ReadLine();

            var selectedAccount = currentUser.Accounts.FirstOrDefault(a => a.Name == input1 || a.Id.ToString() == input1);

            if (selectedAccount != null)
            {
                Console.WriteLine("Enter the amount to deposit");

                if (double.TryParse(Console.ReadLine(), out double depositAmount))
                {
                    selectedAccount.Balance += depositAmount;
                    currentContext.SaveChanges();
                    Console.WriteLine($"Deposit successfull. Updated balance: {selectedAccount.Balance}");
                }
                else
                {
                    Console.WriteLine("Insufficent funds. ");
                }
            }
            else
            {
                Console.WriteLine("No account matching the criteria found.");
            }
        }

        private static void OpenNewAccount()
        {
            Console.Write("What would you like the account name to be?");
            string input1 = Console.ReadLine();

            Account newAccount = new Account()
            {
                Name = input1,
                User = currentUser,
                UserId = currentUser.Id,
            };

            bool success = DbHelper.AddAccount(currentContext, newAccount);

            if (success)
            {
                Console.WriteLine($"Successfully created account {input1}.");
            }
            else
            {
                Console.WriteLine($"Failed to create account {input1}");
            }
        }

        private static void LogOut()
        {
            Console.WriteLine("Logging out...");
        }
    }
}
