using BankBootstrap.Data;
using BankBootstrap.Utilities;

namespace BankBootstrap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to NET23Bank");
            Console.WriteLine("Please log in");

            Console.Write("Enter username: ");
            string userName = Console.ReadLine();
            
            Console.Write("Enter pin: ");
            string pin = Console.ReadLine();

            if (userName == "admin")
            {
                if(pin != "1234")
                {
                    Console.WriteLine("Wrong pin!");
                    return;
                }

                AdminFunctions.DoAdminTasks();
                return;
            }

            using (BankContext context = new BankContext())
            {
                bool userExists = context.Users.Any(u => u.Name == userName);
                bool pinExists = context.Users.Any(p => p.Pin == pin);

                if (userExists && pinExists)
                {
                    Console.WriteLine($"Logged in to user {userName}");
                }
                else
                {
                    Console.WriteLine("No user with that user name or pin exists");
                }

            }

            

            //user login here
        }
    }
}