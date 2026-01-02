using ASPlogInV2.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace ASPlogInV2.Helpers
{
    using BCrypt.Net;
    public class LogInHelper
    {
        //Check if the values are null
        public static bool isValuesNull(string username, string password)
        {
            try {
                if (username == null || password == null)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message error message:\n{ex.Message}\n\nRaw error message:\n{ex}");
                return true;
            }
        }

        //Check if account exsists
        public static string isAccountExsit(string? username, string? password)
        { 
            //Checks if values are null
            bool areValuesNull = isValuesNull(username, password);
            if (areValuesNull)
            {
                return "No fields can be left blank or empty";
            }
            else
            {
                //Check if username is in database.
                try
                {
                    using (var db = new DbConnectorSQLite())
                    {
                        //Is username in database?
                        var UserAccount = db.UserAccounts.Where(a => a.UserName == username).FirstOrDefault();
                        //User Not found
                        if (UserAccount == null) 
                        {
                            return "Account not found. Please check the username/email is correct or create an account if you don't have one already.";
                        }
                        //User account found
                        else 
                        {
                            //Checks if password entered is correct
                            if (BCrypt.Verify(password, UserAccount.Password))
                            {
                                // Blank|null string means it was successful
                                return "";
                            }
                            //Password incorrect
                            else
                            {
                                return "Incorrect password, please try again.";
                            }
                        }
                    }
                }
                //Something went wrong or database couldn't be read.
                catch (Exception ex) 
                {
                    Console.WriteLine($"Something went wrong, error message:\n{ex.Message}\n\nRaw error message:{ex}");
                    return "Something went wrong, please try again or try later.";
                }
                
            }
        }

    }
}
