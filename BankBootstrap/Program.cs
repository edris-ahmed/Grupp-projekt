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

            //user login here
        }
    }
}