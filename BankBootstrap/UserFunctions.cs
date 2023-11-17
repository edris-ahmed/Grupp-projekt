using BankBootstrap.Data;
using BankBootstrap.Models;
using BankBootstrap.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBootstrap
{
    internal static class UserFunctions 
    {
        private static void PerfromUserChoices(User user, BankContext context)
        {
            string choice = "";
            string input1 = "";
            string input2 = "";
            Account selectedAccount = null;
            Account selectedAccounts1 = null;
            Account selectedAccounts2 = null;

            while (choice != "6")
            {
                Console.WriteLine($"[1] See your accounts and balance");
                Console.WriteLine($"[2] Transfer money between accounts");
                Console.WriteLine($"[3] Withdraw");
                Console.WriteLine($"[4] Deposit");
                Console.WriteLine($"[5] Open new account");
                Console.WriteLine($"[6] Log out");

                Console.Write("Enter an option: ");
                choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "2":
                        Console.Write("Choose the account to take money from enter [Name] or [Id]: ");
                        input1 = Console.ReadLine();
                        Console.Write("Choose the account to transfer money to enter [Name] or [Id]: ");
                        input2 = Console.ReadLine(); 

                        selectedAccounts1 = user.Accounts.FirstOrDefault(a => a.Name == input1 || a.Id.ToString() == input1);
                        selectedAccounts2 = user.Accounts.FirstOrDefault(a => a.Name == input2 || a.Id.ToString() == input2);

                        if (selectedAccounts1 != null && selectedAccounts2 != null)
                        {
                            Console.WriteLine("Enter the transfer amount: ");

                            if (double.TryParse(Console.ReadLine(), out double transferAmount))
                            {
                                if (selectedAccounts1.Balance >= transferAmount)
                                {
                                    selectedAccounts1.Balance -= transferAmount;
                                    selectedAccounts2.Balance += transferAmount;
                                    context.SaveChanges();
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
                        break;

                    case "3":
                        Console.WriteLine("Select the account to withdraw from (enter the account [Name] or [Id]");
                        input1 = Console.ReadLine();

                        selectedAccount = user.Accounts.FirstOrDefault(a => a.Name == input1 || a.Id.ToString() == input1);

                        if (selectedAccount != null)
                        {
                            Console.Write("Enter the amount to withdraw: ");

                            if (double.TryParse(Console.ReadLine(), out double withdrawAmount))
                            {
                                if (selectedAccount.Balance >= withdrawAmount)
                                {
                                    selectedAccount.Balance -= withdrawAmount;
                                    context.SaveChanges();
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
                        break;

                    case "4":
                        Console.WriteLine("Select the account to deposit into (enter the account [Name] or [Id]");
                        input1 = Console.ReadLine();

                        selectedAccount = user.Accounts.FirstOrDefault(a => a.Name == input1 || a.Id.ToString() == input1);

                        if (selectedAccount != null)
                        {
                            Console.WriteLine("Enter the amount to deposit");

                            if (double.TryParse(Console.ReadLine(), out double depositAmount))
                            {
                                selectedAccount.Balance += depositAmount;
                                context.SaveChanges();
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
                        break;

                    case "5":
                        Console.Write("What would you like the account name to be?");
                        input1 = Console.ReadLine();

                        Account newAccount = new Account()
                        {
                            Name = input1,
                            User = user,
                            UserId = user.Id,
                        };

                        bool success = DbHelper.AddAccount(context, newAccount);

                        if (success)
                        {
                            Console.WriteLine($"Successfully created account {input1}.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to create account {input1}");
                        }
                        break;
                        
                    case "6":
                        Console.WriteLine("Logging out...");
                        break;
                }
            
            }

        }
    }
}
