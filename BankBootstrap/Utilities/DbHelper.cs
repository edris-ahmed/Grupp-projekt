using BankBootstrap.Data;
using BankBootstrap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBootstrap.Utilities
{
    internal static class DbHelper
    {
        public static List<User> GetAllUsers(BankContext context) 
        {
            List<User> users = context.Users.ToList();
            return users;
        }

        public static bool AddUser(BankContext context, User user)
        {
            context.Users.Add(user);
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex}");
                return false;
            }
            return true;
        }
    }
}
