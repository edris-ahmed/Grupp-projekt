using BankBootstrap.Data;
using BankBootstrap.Models;
using BankBootstrap.Utilities;

namespace BankBootstrap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Welcome to NET23Bank");
                Console.Write("Would you like to log in or quit the program? Type [Login] to log in or [Quit] to quit: ");

                string userChoice = Console.ReadLine().ToLower();

                if (userChoice == "quit")
                {
                    return;
                }

                Console.Write("Enter username: ");
                string userName = Console.ReadLine();

                Console.Write("Enter pin: ");
                string pin = Console.ReadLine();

                if (userName == "admin")
                {
                    if (pin != "1234")
                    {
                        for (int i = 2; i > 0; i--)
                        {
                            Console.WriteLine("Wrong pin! Tries remaining " + i + ".");
                            Console.Write("Enter pin: ");
                            pin = Console.ReadLine();

                            if (pin == "1234")
                            {
                                AdminFunctions.DoAdminTasks();
                                continue;
                            }
                        }
                        continue;
                    }

                    AdminFunctions.DoAdminTasks();
                    continue;
                }

                using (BankContext context = new BankContext())
                {
                    User existingUser = context.Users.FirstOrDefault(u => u.Name.ToLower() == userName.ToLower() && u.Pin == pin);
                  
                    if (existingUser != null)
                    {
                        Console.WriteLine($"\nLogged in to user {userName.ToUpper()}\n");
                        UserFunctions.PerformUserChoices(existingUser, context);
                    }
                    else
                    {
                        Console.WriteLine("No user with that username or pin exists");
                    }
                }
            }
        }
    }
}