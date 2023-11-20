using BankBootstrap.Data;
using BankBootstrap.Models;
using BankBootstrap.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BankBootstrap
{
    internal static class UserFunctions 
    {
        private static User currentUser;
        private static BankContext currentContext;
        private static string choice;
        public static void PerformUserChoices(User user, BankContext context)
        {
            currentUser = user;
            currentContext = context;
            choice = "";

            while (choice != "6")
            {
                DisplayOptions();

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

                    default:
                        break;
                }          
            }
        }

        private static string DisplayOptions()
        {
            Console.WriteLine($"[1] See your accounts and balance");
            Console.WriteLine($"[2] Transfer money between accounts");
            Console.WriteLine($"[3] Withdraw");
            Console.WriteLine($"[4] Deposit");
            Console.WriteLine($"[5] Open new account");
            Console.WriteLine($"[6] Log out");

            Console.Write("Enter an option using [1] [2] [3] [4] [5] [6]: ");
            
            choice = Console.ReadLine();

            Console.WriteLine();
            return choice;
        }

        private static void ShowAccountBalance()
        {
            if (currentUser.Accounts.Count() == 0)
            {
                Console.WriteLine("Create an account first.");
            }
            else
            {
                Console.WriteLine("Your accounts and balances:22");
                foreach (var account in currentUser.Accounts)
                {
                    Console.WriteLine($"Account: {account.Name}\nBalance: {account.Balance}\n");
                }
            }
        }

        private static void TransferBetweenAccounts()
        {
            bool exitLoop = false;

            while (!exitLoop)
            {
                Console.Write("Choose the account to take money from enter [Name] or [Id] or [Quit] to go back to the options: ");
                string input1 = Console.ReadLine();

                if (input1.ToLower() == "quit")
                {
                    exitLoop = true;
                    break;
                }

                Console.Write("Choose the account to transfer money to enter [Name] or [Id] or [Quit] to go back to the options: ");
                string input2 = Console.ReadLine();

                if (input2.ToLower() == "quit")
                {
                    exitLoop = true;
                    break;
                }

                var selectedAccounts1 = currentContext.Accounts.FirstOrDefault(a => a.Name.ToLower() == input1.ToLower() || a.Id.ToString() == input1);
                var selectedAccounts2 = currentContext.Accounts.FirstOrDefault(a => a.Name.ToLower() == input2.ToLower() || a.Id.ToString() == input2);

                if (selectedAccounts1 != null && selectedAccounts2 != null)
                {
                    Console.Write("Enter the transfer amount: ");

                    if (double.TryParse(Console.ReadLine(), out double transferAmount))
                    {
                        if (transferAmount > 0)
                        {
                            if (selectedAccounts1.Balance >= transferAmount)
                            {
                                selectedAccounts1.Balance -= transferAmount;
                                selectedAccounts2.Balance += transferAmount;
                                currentContext.SaveChanges();
                                Console.WriteLine($"Transfer successfull. Updated balance for account {selectedAccounts2.Name} is {selectedAccounts2.Balance}\n");
                                exitLoop = true;
                            }
                            else
                            {
                                Console.WriteLine("Insufficent funds.\n");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.\n");
                    }
                }
                else
                {
                    Console.WriteLine("One or both of the accounts did not match the criteria.\n");
                }
            }
        }

        private static void WithdrawFromAccount()
        {
            bool exitLoop = false;

            while (!exitLoop)
            {
                Console.Write("Select the account to withdraw from (enter the account [Name] or [Id] or [Quit] to go back to the options: ");
                string input1 = Console.ReadLine();

                if (input1.ToLower() == "quit")
                {
                    exitLoop = true;
                    break;
                }

                var selectedAccount = currentContext.Accounts.FirstOrDefault(a => a.Name.ToLower() == input1.ToLower() || a.Id.ToString() == input1);

                if (selectedAccount != null)
                {
                    Console.Write("Enter the amount to withdraw: ");

                    if (double.TryParse(Console.ReadLine(), out double withdrawAmount))
                    {
                        if (withdrawAmount > 0)
                        {
                            if (selectedAccount.Balance >= withdrawAmount)
                            {
                                selectedAccount.Balance -= withdrawAmount;
                                currentContext.SaveChanges();
                                Console.WriteLine($"Withdrawal successfull. Updated balance: {selectedAccount.Balance}\n");
                                exitLoop = true;
                            }
                            else
                            {
                                Console.WriteLine("Insufficient funds.\n");
                            }
                        }                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.\n");
                    }
                }
                else
                {
                    Console.WriteLine("No account matching the criteria found.\n");
                }
            }
        }

        private static void DepositToAccount()
        {
            bool exitLoop = false;

            while (!exitLoop)
            {
                Console.Write("Select the account to deposit into (enter the account [Name] or [Id] or [Quit] to go back to the options: ");
                string input1 = Console.ReadLine();

                if (input1.ToLower() == "quit")
                {
                    exitLoop = true;
                    break;
                }

                var selectedAccount = currentContext.Accounts.FirstOrDefault(a => a.Name.ToLower() == input1 || a.Id.ToString() == input1);

                if (selectedAccount != null)
                {
                    Console.Write("Enter the amount to deposit: ");

                    if (double.TryParse(Console.ReadLine(), out double depositAmount))
                    {
                        if (depositAmount > 0)
                        {
                            selectedAccount.Balance += depositAmount;
                            currentContext.SaveChanges();
                            Console.WriteLine($"Deposit successfull. Updated balance: {selectedAccount.Balance}\n");
                            exitLoop = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.\n");
                    }
                }
                else
                {
                    Console.WriteLine("No account matching the criteria found.\n");
                }
            }
            
        }

        private static void OpenNewAccount()
        {
            bool exitLoop = false;

            while (!exitLoop)
            {
                Console.Write("What would you like the account name to be? ");
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
                    Console.WriteLine($"Successfully created account {input1}.\n");
                    exitLoop = true;
                }
                else
                {
                    Console.WriteLine($"Failed to create account {input1}");
                    Console.WriteLine("Would you like to try again? [No] To go back to options menu and [Yes] to try agian.");
                    input1 = Console.ReadLine().ToLower();

                    while (input1 != "yes" || input1 != "no")
                    {
                        Console.WriteLine("Please answer with either [Yes] to try again or [No] to go back to the options menu");
                    }

                    if (input1 == "no")
                    {
                        Console.WriteLine();
                        exitLoop = true;
                    }
                }
            }  
        }

        private static void LogOut()
        {
            Console.WriteLine("Logging out...\n");
        }
    }
}
