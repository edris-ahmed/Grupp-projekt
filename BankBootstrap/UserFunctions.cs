using BankBootstrap.Data;
using BankBootstrap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBootstrap
{
    internal static class UserFunctions 
    {
        private static void UserChoices(User user, BankContext context)
        {
            string choice = "";
            
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
                    case "3":
                        
                        Console.WriteLine("Select the account to withdraw from");
                        string input = Console.ReadLine();

                        Console.WriteLine("Select the account to withdraw from (enter the account [Name] or [Id]");


                        bool accountExists = context.Users
                            .Any(a => a.Id.ToString() == searchValue || a.Name == searchValue);

                        if (accountExists)
                        {
                            Console.Write("Enter the amount to withdraw: ");

                            if (double.TryParse(Console.ReadLine(), out double withdrawAmount))
                            {
                                if (account.Balance >= withdrawAmount)
                                {
                                    account.Balance -= withdrawAmount;
                                    context.SaveChanges();
                                    Console.WriteLine($"Withdrawal successfull. Updated balance: {account.Balance}");
                                }
                                else
                                {
                                    Console.WriteLine("Insufficient funds. ");
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
                        {
                            Console.Write("Enter the amount to deposit: ");
                            if (double.TryParse(Console.ReadLine(), out double depositAmount))
                            {
                                account.Balance += depositAmount;
                                context.SaveChanges();
                                Console.WriteLine($"Deposit successfull. Updated Balance: {account.Balance}");
                            }
                            break;
                        }
                    case "6":
                        Console.WriteLine("Logging out...");
                        break;
                }
            
            }

        }
    }
}
