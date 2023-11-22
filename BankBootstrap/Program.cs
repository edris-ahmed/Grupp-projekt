using BankBootstrap.Data;
using BankBootstrap.Models;
using Microsoft.EntityFrameworkCore;

namespace BankBootstrap
{
    internal class Program
    {
        private static int consecutiveFailures = 0;
        private static DateTime cooldownStartTime = DateTime.MinValue;
        private static readonly int maxConsecutiveFailures = 3;
        private static readonly int cooldownDurationMinutes = 3;
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Welcome to NET23Bank");
                Console.Write("Would you like to log in or quit the program? Type [Login] to log in or [Quit] to quit: ");

                string userChoice = Console.ReadLine().ToLower();

                if (userChoice == "quit")
                {
                    break;
                }

                Console.Write("Enter username: ");
                string userName = Console.ReadLine();

                Console.Write("Enter pin: ");
                string pin = Console.ReadLine();

                if (userName == "admin")
                {
                    if (pin == "1234")
                    {
                        AdminFunctions.DoAdminTasks();
                        continue;
                    }

                    if (pin != "1234")
                    {
                        consecutiveFailures++;

                        for (int i = 2; i > 0; i--)
                        {
                            Console.WriteLine("Wrong pin! Tries remaining " + i + ".");
                            Console.Write("Enter pin: ");
                            pin = Console.ReadLine();

                            
                            consecutiveFailures++;
                        }

                        if (consecutiveFailures == 3)
                        {
                            Console.WriteLine($"To many consecutive failures. Cooldown for 3 minutes. ");
                            Thread.Sleep(consecutiveFailures * 60 * 1000);
                            consecutiveFailures = 0;
                        }

                        Console.WriteLine("Redirecting to start...\n");
                        continue;
                    }
                }

                using (BankContext context = new BankContext())
                {
                    User existingUser = context.Users
                        .Include(u => u.Accounts)
                        .FirstOrDefault(u => u.Name.ToLower() == userName.ToLower() && u.Pin == pin);
                  
                    if (existingUser != null)
                    {
                        Console.WriteLine($"\nLogged in to user {userName.ToUpper()}\n");
                        UserFunctions.PerformUserChoices(existingUser, context);
                    }
                    else
                    {
                        Console.WriteLine("\nNo user with that username or pin exists.\n");
                        consecutiveFailures++;

                        for (int i = 2; i > 0; i--)
                        {
                            Console.WriteLine($"Tries remaining: {i}");

                            Console.Write("Enter username: ");
                            userName = Console.ReadLine();

                            Console.Write("Enter pin: ");
                            pin = Console.ReadLine();

                            if (userName == "admin" && pin == "1234")
                            {
                                AdminFunctions.DoAdminTasks();
                                break;
                            }

                            Console.WriteLine();

                            existingUser = context.Users
                           .Include(u => u.Accounts)
                           .FirstOrDefault(u => u.Name.ToLower() == userName.ToLower() && u.Pin == pin);

                            if (existingUser != null)
                            {
                                consecutiveFailures = 0;
                                UserFunctions.PerformUserChoices(existingUser, context);
                                break;
                            }
                            consecutiveFailures++;
                        }

                        if (consecutiveFailures == 3)
                        {
                            Console.WriteLine($"To many consecutive failures. Cooldown for 3 minutes. ");
                            Thread.Sleep(consecutiveFailures * 60 * 1000);
                            consecutiveFailures = 0;
                        }

                        Console.WriteLine("Redirecting to start...\n");
                        continue;
                    }
                }
            }

            Console.WriteLine();
            Console.Write("Press any key to quit: ");
            Console.ReadKey();
        }
    }
}