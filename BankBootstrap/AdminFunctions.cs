using BankBootstrap.Utilities;
using BankBootstrap.Models;
using BankBootstrap.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Linq.Expressions;

namespace BankBootstrap
{
    internal static class AdminFunctions
    {
        public static void DoAdminTasks()
        {
            using (BankContext context = new BankContext())
            {
                Console.WriteLine("\nCurrent users in system:");
                List<User> users = DbHelper.GetAllUsers(context);

                foreach (User user in users)
                {
                    Console.WriteLine($"{user.Name}");
                }

                Console.WriteLine($"Total number of users = {users.Count()}\n");
                Console.WriteLine("[C] to create new user");
                Console.WriteLine("[X] to exit back to [Main]");

                while (true)
                {
                    Console.Write("Enter command: ");
                    string command = Console.ReadLine().ToLower();

                    switch (command)
                    {
                        case "c":
                            CreateUser(context);
                            break;

                        case "x":
                            Console.WriteLine();
                            return;

                        default:
                            Console.WriteLine($"Unknown input: {command}");
                            break;
                    }
                }
            }
        }

        private static void CreateUser(BankContext context)
        {
            Console.WriteLine("Create user");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Random random = new Random();
            string pin = random.Next(1000, 10000).ToString();

            User newUser = new User()
            {
                Name = username,
                Pin = pin
            };
            bool success = DbHelper.AddUser(context, newUser);
            
            if (success)
            {
                Console.WriteLine($"Created user {username} with pin: {pin}");
            }
            else
            {
                Console.WriteLine($"Failed to create user with username {username}");
            }
        }

    }
}
