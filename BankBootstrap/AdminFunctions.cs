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
using Microsoft.IdentityModel.Tokens;

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

                while (true)
                {
                    Console.WriteLine("[C] to create new user");
                    Console.WriteLine("[D] to delete user");
                    Console.WriteLine("[L] to see the list of users again");
                    Console.WriteLine("[X] to exit back to [Main]");

                    Console.Write("Enter command: ");
                    string command = Console.ReadLine().ToLower();

                    switch (command)
                    {
                        case "c":
                            CreateUser(context);
                            break;

                        case "d":
                            DeleteUser(context);
                            break;

                        case "l":
                            ListOfUsers(context);
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
            Console.WriteLine("\nCreate USER");
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
                Console.WriteLine($"Created user {username} with pin: {pin}\n");
            }
            else
            {
                Console.WriteLine($"Failed to create user with username {username}");
            }

            Console.Write("Press enter to get back to the menu: ");
            Console.ReadLine();
        }

        private static void DeleteUser(BankContext context)
        {
            while (true)
            {
                Console.Write("Enter the name of the user you want to delete: ");
                string input = Console.ReadLine();

                var userToDelete = context.Users.FirstOrDefault(u => u.Name.ToLower() == input.ToLower());

                if (userToDelete != null)
                {
                    context.Users.Remove(userToDelete);
                    context.SaveChanges();
                    Console.WriteLine($"Successfully deleted user: {userToDelete.Name}\n");

                    Console.Write("Press enter to get back to the menu: ");
                    Console.ReadLine();
                    break;
                }
                else
                {
                    Console.WriteLine("No account matching the criteria.\n");
                }
            }

           
            

            //if (context.Entry(user).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
            //{
            //    context.Users.Attach(user);
            //}

            
        }
        private static void ListOfUsers(BankContext context)
        {
            Console.WriteLine("\nCurrent users in system:");
            List<User> users = DbHelper.GetAllUsers(context);

            foreach (User user in users)
            {
                Console.WriteLine($"{user.Name}");
            }

            Console.WriteLine($"Total number of users = {users.Count()}\n");

            Console.Write("Press enter to get back to the menu: ");
            Console.ReadLine();
            Console.WriteLine();
        }
    }
}
