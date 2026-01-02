using ASPlogInV2.Models;
using NuGet.Protocol;

namespace ASPlogInV2.Helpers
{
    using BCrypt.Net;
    public class ChangePasswordHelper
    {
        public static bool isEmailInDB(string email)
        {
            try
            {
                //Checkes if the email exists in database
                using (var db = new DbConnectorSQLite())
                {
                    var accountDetails = db.UserAccounts.FirstOrDefault(x => x.UserEmail == email);
                    //If it has value then the email exsits
                    if (accountDetails != null) 
                    {
                        return true;
                    }

                    //If null, email isn't in database
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error message:\n{ex.Message}\n\nRaw error:\n{ex}");
                return false;
            }
        }

        //Function to see if password or confirm password is null
        public static bool isInputFieldsNull(string email, string newPassword, string confirmPassword)
        {
            //If fields are empty or null
            if (email == "" || email == null || newPassword == "" || confirmPassword == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Function to varify if the password is good enough
        public static bool isPasswordGood(string userPassword)
        {
            try
            {
                //Checks if string has at least one digit
                bool hasDigit = false;
                bool haslowerchar = false;
                bool hasupperchar = false;
                foreach (char c in userPassword)
                {
                    //Check if there is a number
                    if (char.IsDigit(c))
                    {
                        hasDigit = true;
                    }
                    //Check if there is a lowercase letter
                    if (char.IsLower(c))
                    {
                        haslowerchar = true;
                    }
                    //Check if there is an uppercase letter
                    if (char.IsUpper(c))
                    {
                        hasupperchar = true;
                    }

                    //To stop once digit, lower and upper case requirements are found.
                    if (hasDigit && haslowerchar && hasupperchar)
                    {
                        break;
                    }
                }

                //If password valid
                if (userPassword.Count() >= 8 && hasDigit && haslowerchar && hasupperchar)
                {
                    return true;
                }
                //Pasword doesn't meet minimum requirements
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error message:\n{ex.Message}\n\nRaw error message:\n{ex}");
                return false;
            }
        }

        //Function to change user account's password.
        public static string changeUserPassword(string email, string newPassword, string confirmPassword)
        {
            try {
                //Checks if password and confirm password fields were empty
                if (isInputFieldsNull(email, newPassword, confirmPassword))
                {
                    return "Input fields cannot be left empty or blank.";
                }
                //Checks if the password doesn't meet the minimum requirements
                else if (!isPasswordGood(newPassword))
                {
                    return "The password must be at least 8 characters long, have one digit and one uppercase and lowercase letters.";
                }
                //Checks if newPassword matches confirmPassword
                else if (newPassword != confirmPassword)
                {
                    return "Password does not match confirm password.";
                }
                else
                {
                    //Change to password to new password
                    using (var db = new DbConnectorSQLite())
                    {
                        //Hash new password
                        var hashNewPassword = BCrypt.HashPassword(newPassword);
                        //Grab user account
                        var userAccount = db.UserAccounts.Where(a => a.UserEmail == email).FirstOrDefault();
                        //Change old password to new password. (With hashing)
                        userAccount.Password = hashNewPassword;
                        db.SaveChanges(); //Save the changes so it'll update database.
                    }

                    //Empty string means it is successful.
                    return "";
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error message:\n{ex.Message}\n\nRaw error:\n{ex}");
                return "Something went wrong, please try again.";
            }
        }

    }
}
