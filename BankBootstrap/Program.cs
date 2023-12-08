using BankBootstrap.Data;
using BankBootstrap.Models;
using Microsoft.EntityFrameworkCore;

namespace BankBootstrap
{
    internal class Program
    {
        // Antal felaktiga försök och cooldown-variabler
        private static int consecutiveFailures = 0;
        private static DateTime cooldownStartTime = DateTime.MinValue;
        private static readonly int maxConsecutiveFailures = 3;
        private static readonly int cooldownDurationMinutes = 3;
        static void Main(string[] args)
        {
            // Huvudloopen för programmet
            while (true)
            {
                Console.WriteLine("Welcome to NET23Bank");
                Console.Write("Would you like to log in or quit the program? Type [Login] to log in or [Quit] to quit: ");

                string userChoice = Console.ReadLine().ToLower();

                // Avsluta programmet om användaren väljer att sluta
                if (userChoice == "quit")
                {
                    break;
                }

                Console.Write("Enter username: ");
                string userName = Console.ReadLine();

                Console.Write("Enter pin: ");
                string pin = Console.ReadLine();

                // Inloggning för administratören
                if (userName == "admin")
                {
                    if (pin == "1234")
                    {
                        AdminFunctions.DoAdminTasks();
                        continue;
                    }

                    if (pin != "1234")
                    {
                        // Hantera felaktiga pinkoder
                        consecutiveFailures++;

                        for (int i = 2; i > 0; i--)
                        {
                            Console.WriteLine("Wrong pin! Tries remaining " + i + ".");
                            Console.Write("Enter pin: ");
                            pin = Console.ReadLine();

                            
                            consecutiveFailures++;
                        }

                        // Om det finns för många felaktiga försök, vänta i en viss tid
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
                
                // Använd Entity Framework för att söka efter befintlig användare
                using (BankContext context = new BankContext())
                {
                    User existingUser = context.Users
                        .Include(u => u.Accounts)
                        .FirstOrDefault(u => u.Name.ToLower() == userName.ToLower() && u.Pin == pin);
                  
                    if (existingUser != null)
                    {
                        // Användarval efter inloggning
                        Console.WriteLine($"\nLogged in to user {userName.ToUpper()}\n");
                        UserFunctions.PerformUserChoices(existingUser, context);
                    }
                    else
                    {
                        // Hantera felaktiga inloggningsförsök för användare
                        Console.WriteLine("\nNo user with that username or pin exists.\n");
                        consecutiveFailures++;

                        for (int i = 2; i > 0; i--)
                        {
                            Console.WriteLine($"Tries remaining: {i}");

                            Console.Write("Enter username: ");
                            userName = Console.ReadLine();

                            Console.Write("Enter pin: ");
                            pin = Console.ReadLine();

                            // Kontrollera om användaren är administratör efter felaktiga försök
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

                        // Om det finns för många felaktiga försök, vänta i en viss tid
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